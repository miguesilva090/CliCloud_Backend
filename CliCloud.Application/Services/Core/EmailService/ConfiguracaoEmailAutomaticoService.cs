using System.Net;
using System.Net.Mail;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Core.EmailService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Core.Email;
using Microsoft.Extensions.Logging;

namespace CliCloud.Application.Services.Core.EmailService;

public class ConfiguracaoEmailAutomaticoService(
    IRepositoryAsync repository,
    IServicoEmailAutomaticoDados dados,
    ILogger<ConfiguracaoEmailAutomaticoService> logger
) : IConfiguracaoEmailAutomaticoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IServicoEmailAutomaticoDados _dados = dados;
    private readonly ILogger<ConfiguracaoEmailAutomaticoService> _logger = logger;

    public async Task ExecutarAsync(CancellationToken cancellationToken = default)
    {
        var clinicas = await _repository.GetListAsync<Clinica, Guid>(cancellationToken: cancellationToken);
        foreach (var clinica in clinicas)
        {
            if (cancellationToken.IsCancellationRequested) break;
            try
            {
                await ProcessarClinicaAsync(clinica.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no batch Email automático da clínica {ClinicaId}", clinica.Id);
            }
        }
    }

    private async Task ProcessarClinicaAsync(Guid clinicaId, CancellationToken ct)
    {
        var cfg = (await _repository.GetListAsync<ConfiguracaoEmail, Guid>(new ConfiguracaoEmailPorClinicaSpec(clinicaId), ct)).FirstOrDefault();
        if (cfg == null) return;
        if (string.IsNullOrWhiteSpace(cfg.Server) || string.IsNullOrWhiteSpace(cfg.Username) || string.IsNullOrWhiteSpace(cfg.Password) || cfg.Porta <= 0) return;

        var regras = await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(new ConfiguracoesEmailAutomaticasPorClinicaSpec(clinicaId), ct);
        foreach (var regra in regras.Where(r => r.Ativo == 1))
        {
            var template = (regra.Textomensagem ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(template))
            {
                await RegistarErroAsync(clinicaId, regra.Codigo, "Template vazio.");
                continue;
            }

            var dataAlvo = DateTime.Today.AddDays(regra.Diasantecedencia);
            var eventos = await ObterEventosPorCodigoAsync(clinicaId, regra.Codigo, dataAlvo, template, ct);
            foreach (var evento in eventos)
                await EnviarEventoAsync(cfg, regra.Descricao, evento);
        }
    }

    private Task<List<EmailAutomaticoEventoDTO>> ObterEventosPorCodigoAsync(Guid clinicaId, string codigo, DateTime dataAlvo, string template, CancellationToken ct)
        => codigo switch
        {
            "1" => _dados.ObterEventosConsultasAsync(clinicaId, dataAlvo, template, ct),
            "2.1" => _dados.ObterEventosPrimeiraSessaoAsync(clinicaId, dataAlvo, template, ct),
            "2.2" => _dados.ObterEventosSessoesAsync(clinicaId, dataAlvo, template, ct),
            "2.3" => _dados.ObterEventosSessoesFinaisAsync(clinicaId, dataAlvo, template, ct),
            "3" => _dados.ObterEventosAniversariosAsync(clinicaId, dataAlvo, template, ct),
            "6.1" => _dados.ObterEventosPedidoAgendamentoAsync(clinicaId, dataAlvo, template, ct),
            "6.2" => _dados.ObterEventosPedidoAgendamentoConfirmadoAsync(clinicaId, dataAlvo, template, ct),
            "7.1" => _dados.ObterEventosAulasAsync(clinicaId, dataAlvo, template, ct),
            _ => Task.FromResult(new List<EmailAutomaticoEventoDTO>())
        };

    private async Task EnviarEventoAsync(ConfiguracaoEmail cfg, string? descricaoRegra, EmailAutomaticoEventoDTO evento)
    {
        var destino = (evento.EmailDestino ?? string.Empty).Trim();
        var corpo = RenderTemplate(evento.MensagemTemplate, evento.Placeholders);
        var assunto = string.IsNullOrWhiteSpace(descricaoRegra) ? (string.IsNullOrWhiteSpace(evento.Assunto) ? "Notificação" : evento.Assunto) : descricaoRegra;

        var historico = new HistoricoEmail
        {
            ClinicaId = cfg.ClinicaId,
            AssuntoEmail = assunto,
            CorpoEmail = corpo,
            EmailDestino = destino,
            DataHoraCriacao = DateTime.Now,
            Status = "Pendente",
            Modulo = evento.Modulo
        };

        await _repository.CreateAsync<HistoricoEmail, Guid>(historico);
        await _repository.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(destino) || string.IsNullOrWhiteSpace(corpo))
        {
            historico.Status = "Erro de validação";
            historico.MensagemErro = "Destino/corpo inválido";
            _ = await _repository.UpdateAsync<HistoricoEmail, Guid>(historico);
            await _repository.SaveChangesAsync();
            return;
        }

        try
        {
            using var smtp = new SmtpClient(cfg.Server, cfg.Porta)
            {
                EnableSsl = cfg.UseSSL,
                Credentials = new NetworkCredential(cfg.Username, cfg.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var fromAddress = string.IsNullOrWhiteSpace(cfg.Email) ? cfg.Username : cfg.Email;
            using var mail = new MailMessage(fromAddress, destino)
            {
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(mail);
            historico.Status = "Enviado";
            historico.DataHoraEnvio = DateTime.Now;
            historico.MensagemErro = null;
        }
        catch (Exception ex)
        {
            historico.Status = "Erro de envio";
            historico.MensagemErro = ex.Message;
        }

        _ = await _repository.UpdateAsync<HistoricoEmail, Guid>(historico);
        await _repository.SaveChangesAsync();
    }

    private async Task RegistarErroAsync(Guid clinicaId, string codigoRegra, string erro)
    {
        var h = new HistoricoEmail
        {
            ClinicaId = clinicaId,
            AssuntoEmail = $"Regra automática {codigoRegra}",
            CorpoEmail = $"Regra automática {codigoRegra}",
            EmailDestino = string.Empty,
            DataHoraCriacao = DateTime.Now,
            Status = "Erro de validação",
            MensagemErro = erro,
            Modulo = $"EmailAutomatico-{codigoRegra}"
        };
        await _repository.CreateAsync<HistoricoEmail, Guid>(h);
        await _repository.SaveChangesAsync();
    }

    private static string RenderTemplate(string template, Dictionary<string, string> values)
    {
        var result = template ?? string.Empty;
        foreach (var kv in values)
            result = result.Replace($"@{kv.Key}", kv.Value ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        return result;
    }
}
