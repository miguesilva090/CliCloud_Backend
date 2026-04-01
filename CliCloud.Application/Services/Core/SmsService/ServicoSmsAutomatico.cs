using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Helpers;
using CliCloud.Application.Services.Core.SmsService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.Extensions.Logging;

namespace CliCloud.Application.Services.Core.SmsService;

public class ServicoSmsAutomatico(
    IRepositoryAsync repository,
    IServicoSmsAutomaticoDados dados,
    ILogger<ServicoSmsAutomatico> logger
) : IServicoSmsAutomatico
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IServicoSmsAutomaticoDados _dados = dados;
    private readonly ILogger<ServicoSmsAutomatico> _logger = logger;

    public async Task ExecutarAsync(CancellationToken cancellationToken = default)
    {
        var clinicas = await _repository.GetListAsync<Clinica, Guid>(cancellationToken: cancellationToken);

        foreach ( var clinica in clinicas)
        {
            if(cancellationToken.IsCancellationRequested) break;

            try
            {
                await ProcessarClinicaAsync(clinica.Id, cancellationToken);
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Erro no batch SMS automático da clínica {ClinicaId}", clinica.Id);
            }
        }
    }

    private async Task ProcessarClinicaAsync(Guid clinicaId, CancellationToken ct)
    {
        var cfg = (await _repository.GetListAsync<ConfiguracaoSms, Guid>( new ConfiguracaoSmsPorClinicaSpec(clinicaId), ct)).FirstOrDefault();

        if(cfg == null || !cfg.Ativo || cfg.UsenditArpoone != 2) return;

        if(!cfg.ArpooneOrganizationID.HasValue 
            || string.IsNullOrWhiteSpace(cfg.ArpooneApiKey)
            || string.IsNullOrWhiteSpace(cfg.ArpooneSender)
            || string.IsNullOrWhiteSpace(cfg.ArpooneUrl))
            return;

        var hoje = DateTime.Today;
        if(cfg.ControloSmsAutomaticos.HasValue && cfg.ControloSmsAutomaticos.Value.Date == hoje)
            return;

        var regras = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(
            new ConfiguracoesSmsAutomaticasPorClinicaSpec(clinicaId), ct );
        

        foreach (var regra in regras.Where(r => r.Ativo == 1))
        {
            if (ct.IsCancellationRequested) break;

            var template = (regra.Textomensagem ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(template))
            {
                await RegistarErroValidacaoTemplateAsync(clinicaId, regra.Codigo, "Template vazio para regra automática de SMS");
                continue;
            }

            if (!CodigoRegraSuportado(regra.Codigo))
                continue;

            var dataAlvo = DateTime.Today.AddDays(regra.Diasantecedencia);
            var eventos = await ObterEventosPorCodigoAsync(clinicaId, regra.Codigo, dataAlvo, template, ct);

            foreach (var evento in eventos)
            {
                if (ct.IsCancellationRequested) break;
                await EnviarEventoAsync(cfg, evento, ct);
            }
        }

        cfg.ControloSmsAutomaticos = DateTime.Now;
        _ = await _repository.UpdateAsync<ConfiguracaoSms, Guid>(cfg);
        await _repository.SaveChangesAsync();
    }

    private Task<List<SmsAutomaticoEventoDTO>> ObterEventosPorCodigoAsync(
        Guid clinicaId, string codigo, DateTime dataAlvo, string template, CancellationToken ct)
        => codigo switch
        {
            "1" => _dados.ObterEventosConsultasAsync(clinicaId, dataAlvo, template, ct),
            "2.1" => _dados.ObterEventosPrimeiraSessaoAsync(clinicaId, dataAlvo,template, ct),
            "2.2" => _dados.ObterEventosSessoesAsync(clinicaId, dataAlvo, template, ct),
            "2.3" => _dados.ObterEventosSessoesFinaisAsync(clinicaId, dataAlvo, template, ct),
            "3" => _dados.ObterEventosAniversariosAsync(clinicaId, dataAlvo, template, ct),
            "7.1" => _dados.ObterEventosAulasAsync(clinicaId, dataAlvo, template, ct),
            _ => Task.FromResult(new List<SmsAutomaticoEventoDTO>())
        };

    private async Task EnviarEventoAsync(ConfiguracaoSms cfg, SmsAutomaticoEventoDTO evento, CancellationToken ct)
    {
        if (!cfg.ArpooneOrganizationID.HasValue || string.IsNullOrWhiteSpace(cfg.ArpooneApiKey))
        {
            await RegistarErroValidacaoEventoAsync(evento, "Configuração Arpoone inválida.");
            return;
        }

        var idMensagem = Guid.NewGuid();
        var numero = NormalizarNumeroTelemovel(evento.NumeroDestino);
        var mensagem = RenderTemplate(evento.MensagemTemplate, evento.Placeholders);

        if(string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(mensagem))
        {
            await RegistarErroValidacaoEventoAsync(evento, "Número ou mensagem inválidos");
            return;
        }

        var historico = new HistoricoSms
        {
            ClinicaId = cfg.ClinicaId,
            IdMensagem = idMensagem,
            TextoMensagem = mensagem, 
            NumeroDestinatario = numero, 
            Status = "Pendente",
            DataHoraCriacao = DateTime.Now,
            Modulo = evento.Modulo, 
            CodigoUtente = evento.CodigoUtente,
            CodigoMedico = evento.CodigoMedico,
            CodigoFisioterapeuta = evento.CodigoFisioterapeuta,
            CodigoConsulta = evento.CodigoConsulta,
            CodigoTratamento = evento.CodigoTratamento, 
            CodigoAula = evento.CodigoAula
        };

        await _repository.CreateAsync<HistoricoSms, Guid>(historico);
        await _repository.SaveChangesAsync();

        var apiKey = CriptografiaSmsHelper.Decifrar(cfg.ArpooneApiKey!);

        var payload = new 
        {
            organizationId = cfg.ArpooneOrganizationID.Value.ToString(),
            messages = new[]
            {
                new
                {
                    to = numero,
                    text = mensagem,
                    from = cfg.ArpooneSender,
                    expirationDateTime = DateTime.UtcNow.AddHours(24),
                    smsWebhooks = new 
                    {
                        delivered = new { url = cfg.WebhookDeliveredUrl, enabled = !string.IsNullOrWhiteSpace(cfg.WebhookDeliveredUrl) },
                        notDelivered = new { url = cfg.WebhookNotDeliveredUrl, enabled = !string.IsNullOrWhiteSpace(cfg.WebhookNotDeliveredUrl)},
                        pending = new { url = cfg.WebhookPendingUrl, enabled = !string.IsNullOrWhiteSpace(cfg.WebhookPendingUrl) },
                        customPayload = idMensagem.ToString()
                    }
                }
            }
        };

        using var http = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, cfg.ArpooneUrl);

        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        try
        {
            using var resp = await http.SendAsync(req, ct);
            if (resp.StatusCode == HttpStatusCode.OK || resp.StatusCode == HttpStatusCode.Accepted)
            {
                historico.Status = "Enviado";
                historico.DataHoraEnvio = DateTime.Now;
                historico.MensagemErro = null;
            }
            else
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                historico.Status = "Não Entregue";
                historico.MensagemErro = $"Arpoone HTTP {(int)resp.StatusCode}: {body}";
            }
        }
        catch(Exception ex)
        {
            historico.Status = "Erro de envio";
            historico.MensagemErro = ex.Message;
        }

        _ = await _repository.UpdateAsync<HistoricoSms, Guid>(historico);
        await _repository.SaveChangesAsync();
    }

    private async Task RegistarErroValidacaoTemplateAsync(Guid clinicaId, string codigoRegra, string erro)
    {
        var h = new HistoricoSms
        {
            ClinicaId = clinicaId,
            IdMensagem = Guid.NewGuid(),
            TextoMensagem = $"Regra automática {codigoRegra}",
            NumeroDestinatario = string.Empty,
            Status = "Erro de validação",
            MensagemErro = erro,
            DataHoraCriacao = DateTime.Now,
            Modulo = $"SMS Automatico -{codigoRegra}"
        };

        await _repository.CreateAsync<HistoricoSms, Guid>(h);
        await _repository.SaveChangesAsync();
    }

    private async Task RegistarErroValidacaoEventoAsync(SmsAutomaticoEventoDTO evento, string erro)
    {
        var h = new HistoricoSms
        {
            ClinicaId = evento.ClinicaId, 
            IdMensagem = Guid.NewGuid(),
            TextoMensagem = evento.MensagemTemplate,
            NumeroDestinatario = evento.NumeroDestino, 
            Status = "Erro de validação",
            MensagemErro = erro, 
            DataHoraCriacao = DateTime.Now,
            Modulo = evento.Modulo,
            CodigoUtente = evento.CodigoUtente,
            CodigoMedico = evento.CodigoMedico, 
            CodigoFisioterapeuta = evento.CodigoFisioterapeuta,
            CodigoConsulta = evento.CodigoConsulta,
            CodigoTratamento = evento.CodigoTratamento,
            CodigoAula = evento.CodigoAula
        };

        await _repository.CreateAsync<HistoricoSms, Guid>(h);
        await _repository.SaveChangesAsync();
    }

    private static string RenderTemplate(string template, Dictionary<string, string> values)
    {
        var result = template ?? string.Empty;
        foreach(var kv in values)
            result = result.Replace($"@{kv.Key}", kv.Value ?? string.Empty , StringComparison.OrdinalIgnoreCase);
        return result;
    }

    private static string NormalizarNumeroTelemovel(string numero)
    {
        var d = new string((numero ?? string.Empty).Where(char.IsDigit).ToArray());
        if(string.IsNullOrWhiteSpace(d)) return string.Empty;
        if(d.StartsWith("351", StringComparison.Ordinal) && d.Length == 12) return d;
        if(d.StartsWith('0') && d.Length == 10) return $"351{d[1..]}";
        if(d.Length == 9) return $"351{d}";
        return d;
    }

    private static bool CodigoRegraSuportado(string codigo)
    {
        return codigo == "1"
            || codigo == "2.1"
            || codigo == "2.2"
            || codigo == "2.3"
            || codigo == "3"
            || codigo == "7.1";
    }
}