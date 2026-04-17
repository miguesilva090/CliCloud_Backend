using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Core.EmailService.Filters;
using CliCloud.Application.Services.Core.EmailService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core.Email;
using CliCloud.Application.Services.Core.EmailService.Helpers;

namespace CliCloud.Application.Services.Core.EmailService;

public class ConfiguracaoEmailService(IRepositoryAsync repository) : IConfiguracaoEmailService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfiguracaoEmailDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        var spec = new ConfiguracaoEmailPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoEmail, Guid>(spec)).FirstOrDefault();

        if(entidade == null)
            return ResponseFactory.Fail<ConfiguracaoEmailDTO>("Configuração de email não encontrada");

        return ResponseFactory.Success(Map(entidade));
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoEmailRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Server) || 
            string.IsNullOrWhiteSpace(request.Password) || 
            request.Porta <= 0 ||
            (request.TipoServico != 1 && request.TipoServico != 2))
            {
                return ResponseFactory.Fail<Guid>("Dados inválidos na configuração de email");
            }

        var spec = new ConfiguracaoEmailPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoEmail, Guid>(spec)).FirstOrDefault();

        if(entidade == null)
        {
            entidade = new ConfiguracaoEmail { ClinicaId = clinicaId };
            Aplicar(entidade, request);
            var created = await _repository.CreateAsync<ConfiguracaoEmail, Guid>(entidade);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }

        Aplicar(entidade, request);
        var updated = await _repository.UpdateAsync<ConfiguracaoEmail, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
    }

    public async Task<Response<IEnumerable<ConfiguracaoEmailAutomaticaDTO>>> ObterConfiguracoesAutomaticasAsync(Guid clinicaId)
    {
        var spec = new ConfiguracoesEmailAutomaticasPorClinicaSpec(clinicaId);
        var lista = await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(spec);

        return ResponseFactory.Success(lista.Select(x => new ConfiguracaoEmailAutomaticaDTO
        {
            Id = x.Id,
            ClinicaId = x.ClinicaId,
            Codigo = x.Codigo,
            Descricao = x.Descricao,
            Ativo = x.Ativo,
            Diasantecedencia = x.Diasantecedencia,
            Textomensagem = x.Textomensagem,
        }));
    }

    public async Task<Response<ConfiguracaoEmailAutomaticaDTO>> ObterConfiguracaoAutomaticaAsync(Guid clinicaId, string codigo)
    {
        var spec = new ConfiguracaoEmailAutomaticaPorClinicaSpec(clinicaId, codigo);
        var entidade = (await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(spec)).FirstOrDefault();

        if(entidade == null)
            return ResponseFactory.Fail<ConfiguracaoEmailAutomaticaDTO>("Configuração de email automática não encontrada");

        return ResponseFactory.Success(new ConfiguracaoEmailAutomaticaDTO{
            Id = entidade.Id,
            ClinicaId = entidade.ClinicaId,
            Codigo = entidade.Codigo,
            Descricao = entidade.Descricao, 
            Ativo = entidade.Ativo,
            Diasantecedencia = entidade.Diasantecedencia,
            Textomensagem = entidade.Textomensagem,
        });
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAutomaticaAsync(Guid clinicaId, AtualizarConfiguracaoEmailAutomaticaRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Codigo))
            return ResponseFactory.Fail<Guid>("Código é obrigatório");

        var spec = new ConfiguracaoEmailAutomaticaPorClinicaSpec(clinicaId, request.Codigo);
        var entidade = (await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(spec)).FirstOrDefault();

        if(entidade == null)
        {
            entidade = new ConfiguracaoEmailAutomatica 
            {
                ClinicaId = clinicaId,
                Codigo = request.Codigo
            };
            AplicarAutomatica(entidade, request);
            var created = await _repository.CreateAsync<ConfiguracaoEmailAutomatica, Guid>(entidade);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }

        AplicarAutomatica(entidade, request);
        var updated = await _repository.UpdateAsync<ConfiguracaoEmailAutomatica, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
    }

    public async Task<PaginatedResponse<HistoricoEmailTabelaDTO>> ObterHistoricoPaginadoAsync(Guid clinicaId, HistoricoEmailTabelaFiltro filtro)
    {
        if (filtro.Filters != null && filtro.Filters.Count > 0)
            filtro.PageNumber = 1;

        var order = filtro.Sorting != null ? GSHelpers.GenerateOrderByString(filtro) : "";
        var spec = new HistoricoEmailTabelaSpec(clinicaId, filtro.Filters ?? [], order);
        return await _repository.GetPaginatedResultsAsync<HistoricoEmail, HistoricoEmailTabelaDTO, Guid>(
            filtro.PageNumber,
            filtro.PageSize,
            spec);
    }

    public async Task<Response<Guid>> EnviarEmailPorCodigoAsync(Guid clinicaId, EnviarEmailPorCodigoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.CodigoConfiguracao))
                return ResponseFactory.Fail<Guid>("Código de configuração é obrigatório.");
            if (string.IsNullOrWhiteSpace(request.EmailDestino))
                return ResponseFactory.Fail<Guid>("Email de destinatário inválido.");

            var cfg = (await _repository.GetListAsync<ConfiguracaoEmail, Guid>(
                new ConfiguracaoEmailPorClinicaSpec(clinicaId))).FirstOrDefault();
            if (cfg == null)
                return ResponseFactory.Fail<Guid>("Configuração de Email não encontrada.");

            if (string.IsNullOrWhiteSpace(cfg.Server) ||
                string.IsNullOrWhiteSpace(cfg.Username) ||
                string.IsNullOrWhiteSpace(cfg.Password) ||
                cfg.Porta <= 0)
            {
                return ResponseFactory.Fail<Guid>("Configuração SMTP incompleta.");
            }

            var regra = (await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(
                new ConfiguracaoEmailAutomaticaPorClinicaSpec(clinicaId, request.CodigoConfiguracao))).FirstOrDefault();

            if (regra == null || regra.Ativo != 1 || string.IsNullOrWhiteSpace(regra.Textomensagem))
                return ResponseFactory.Fail<Guid>("Configuração de Email automática não encontrada/ativa.");

            var placeholders = ConstruirPlaceholders(request);
            var corpo = EmailTemplateRenderer.Render(regra.Textomensagem, placeholders);
            var assunto = string.IsNullOrWhiteSpace(regra.Descricao) ? "Notificação" : regra.Descricao.Trim();
            var destino = request.EmailDestino.Trim();

            var historico = new HistoricoEmail
            {
                ClinicaId = clinicaId,
                AssuntoEmail = assunto,
                CorpoEmail = corpo,
                EmailDestino = destino,
                DataHoraCriacao = DateTime.Now,
                Status = "Pendente",
                Modulo = string.IsNullOrWhiteSpace(request.Modulo) ? $"EmailAutomatico-{request.CodigoConfiguracao}" : request.Modulo
            };

            await _repository.CreateAsync<HistoricoEmail, Guid>(historico);
            await _repository.SaveChangesAsync();

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
                    IsBodyHtml = request.CodigoConfiguracao == "6.2"
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

            return historico.Status == "Enviado"
                ? ResponseFactory.Success(historico.Id)
                : ResponseFactory.Fail<Guid>(historico.MensagemErro ?? "Falha no envio de email.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    private static ConfiguracaoEmailDTO Map(ConfiguracaoEmail x) => new()
    {
        Id = x.Id,
        ClinicaId = x.ClinicaId,
        Username = x.Username, 
        Server = x.Server,
        Password = x.Password,
        Porta = x.Porta,
        UseSSL = x.UseSSL,
        TipoServico = x.TipoServico,
        Inbox = x.Inbox,
        Outbox = x.Outbox,
        Email = x.Email,
        DisplayName = x.DisplayName,
        PermitirEliminarEmail = x.PermitirEliminarEmail,
    };

    private static void Aplicar(ConfiguracaoEmail x, AtualizarConfiguracaoEmailRequest r)
    {
        x.Username = r.Username.Trim();
        x.Server = r.Server.Trim();
        x.Password = r.Password;
        x.Porta = r.Porta;
        x.UseSSL = r.UseSSL;
        x.TipoServico = r.TipoServico;
        x.Inbox = r.Inbox?.Trim();
        x.Outbox = r.Outbox?.Trim();
        x.Email = r.Email?.Trim();
        x.DisplayName = r.DisplayName?.Trim();
        x.PermitirEliminarEmail = r.PermitirEliminarEmail;
    }

    private static void AplicarAutomatica(ConfiguracaoEmailAutomatica x, AtualizarConfiguracaoEmailAutomaticaRequest r)
    {
        x.Descricao = r.Descricao.Trim() ?? string.Empty;
        x.Ativo = r.Ativo;
        x.Diasantecedencia = r.Diasantecedencia;
        x.Textomensagem = r.Textomensagem ?? string.Empty;
    }

    private static Dictionary<string, string> ConstruirPlaceholders(EnviarEmailPorCodigoRequest request)
    {
        var data = request.Data?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Utente"] = request.NomeUtente ?? string.Empty,
            ["Data"] = data,
            ["Hora"] = request.Hora ?? string.Empty,
            ["Medico"] = request.NomeMedicoOuProfissional ?? string.Empty,
            ["Médico"] = request.NomeMedicoOuProfissional ?? string.Empty,
            ["Especialidade"] = request.NomeEspecialidade ?? string.Empty,
            ["Fisioterapeuta"] = request.NomeMedicoOuProfissional ?? string.Empty,
            ["Nsessao"] = request.NumeroSessao ?? string.Empty,
            ["NumSessao"] = request.NumeroSessao ?? string.Empty,
            ["Profissional"] = request.NomeMedicoOuProfissional ?? string.Empty,
            ["Modalidade"] = request.NomeEspecialidade ?? string.Empty,
            ["HoraAntiga"] = request.HoraAntiga ?? string.Empty,
            ["DataAntiga"] = request.DataAntiga ?? string.Empty,
            ["HoraNova"] = request.HoraNova ?? request.Hora ?? string.Empty,
            ["DataNova"] = request.DataNova ?? data
        };
    }

    private static string RenderTemplate(string template, Dictionary<string, string> values)
    {
        var result = template ?? string.Empty;
        foreach (var kv in values.OrderByDescending(x => x.Key.Length))
        {
            var escaped = Regex.Escape(kv.Key);
            // Compatibilidade legado: aceita @Token@ e @Token
            result = Regex.Replace(
                result,
                $@"@{escaped}@?",
                kv.Value ?? string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
        return result;
    }

    private static readonly (string Codigo, string Descricao)[] TemplatesFluxoFixos = 
    [
        ("8.1", "Template Consultas"),
        ("8.2", "Template Tratamentos"),
        ("8.3", "Template Exames"),
        ("8.4", "Template Relatórios"),
    ];

    private static string NormalizarCodigoTemplateFluxo(string? codigo)
    {
        var valor = (codigo ?? string.Empty).Trim();
        if(string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        return valor.ToUpperInvariant() switch
        {
            "TPL.CONSULTAS" or "TPLCONSULTAS" => "8.1",
            "TPL.TRATAMENTOS" or "TPLTRATAMENTOS" => "8.2",
            "TPL.EXAMES" or "TPLEXAMES" => "8.3",
            "TPL.RELATORIOS" or "TPLRELATORIOS" => "8.4",
            _ => valor
        };
    }

    public async Task<Response<TemplatesFluxoEmailDTO>> ObterTemplatesFluxoEmailAsync(Guid clinicaId)
    {
        var spec = new ConfiguracoesEmailAutomaticasPorClinicaSpec(clinicaId);
        var lista = await _repository.GetListAsync<ConfiguracaoEmailAutomatica, Guid>(spec);

        var output = new TemplatesFluxoEmailDTO();
        foreach ( var fixo in TemplatesFluxoFixos)
        {
            var reg = lista.FirstOrDefault(x => NormalizarCodigoTemplateFluxo(x.Codigo) == fixo.Codigo);
            output.Templates.Add(new TemplatesFluxoEmailItemDTO{
                Codigo = fixo.Codigo,
                Descricao = reg?.Descricao ?? fixo.Descricao,
                Assunto = reg?.Descricao ?? string.Empty,
                Conteudo = reg?.Textomensagem ?? string.Empty,
                Ativo = reg?.Ativo ?? 1
            });
        }

        return ResponseFactory.Success(output);
    }

    public async Task<Response<bool>> GuardarTemplatesFluxoEmailAsync(Guid clinicaId, AtualizarTemplatesFluxoEmailRequest request)
    {
        if ( request.Templates == null || request.Templates.Count == 0)
            return ResponseFactory.Fail<bool>("Sem templates para guardar");

        var permitidos = TemplatesFluxoFixos.Select(x => x.Codigo).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach ( var item in request.Templates)
        {
            var codigoNormalizado = NormalizarCodigoTemplateFluxo(item.Codigo);
            if (!permitidos.Contains(codigoNormalizado))
                return ResponseFactory.Fail<bool>($"Código inválido: {item.Codigo}");

            // Reutiliza o mesmo fluxo de upsert já estável dos automáticos.
            var guardar = await GuardarConfiguracaoAutomaticaAsync(clinicaId, new AtualizarConfiguracaoEmailAutomaticaRequest
            {
                Codigo = codigoNormalizado,
                Descricao = (item.Assunto ?? string.Empty).Trim(),
                Ativo = item.Ativo,
                Diasantecedencia = 0,
                Textomensagem = item.Conteudo ?? string.Empty
            });

            if (guardar.Status == ResponseStatus.Failure)
            {
                var mensagem = guardar.Messages.TryGetValue("$", out var mensagensCampo) && mensagensCampo.Count > 0
                    ? mensagensCampo[0]
                    : "Não foi possível guardar template de fluxo.";
                return ResponseFactory.Fail<bool>(mensagem);
            }
        }
        return ResponseFactory.Success(true);
    }
}