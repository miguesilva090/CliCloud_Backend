using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Filters;
using CliCloud.Application.Services.Core.SmsService.Helpers;
using CliCloud.Application.Services.Core.SmsService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Globalization;


namespace CliCloud.Application.Services.Core.SmsService
{
    public class ServicoSms(
        IRepositoryAsync repository,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    ) : IServicoSms
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Response<ConfiguracaoSmsDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
        {
            try
            {
                var spec = new ConfiguracaoSmsPorClinicaSpec(clinicaId);
                var lista = await _repository.GetListAsync<ConfiguracaoSms, Guid>(spec);
                var entidade = lista.FirstOrDefault();

                if( entidade == null)
                    return ResponseFactory.Fail<ConfiguracaoSmsDTO>("Configuração de SMS não encontrada");
                
                return ResponseFactory.Success(MapToDTO(entidade, decifrarSegredos: true));
            }
            catch ( Exception ex)
            {
                return ResponseFactory.Fail<ConfiguracaoSmsDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoSmsRequest request)
        {
            try
            {
                var spec = new ConfiguracaoSmsPorClinicaSpec(clinicaId);
                var lista = await _repository.GetListAsync<ConfiguracaoSms, Guid>(spec);
                var entidade = lista.FirstOrDefault();

                if(entidade == null)
                {
                    entidade = new ConfiguracaoSms
                    {
                        ClinicaId = clinicaId,
                        Ativo = request.Ativo,
                    };

                    AplicarConfiguracao(entidade, request);
                    var criado = await _repository.CreateAsync<ConfiguracaoSms, Guid>(entidade);
                    await _repository.SaveChangesAsync();
                    return ResponseFactory.Success(criado.Id);
                }

                AplicarConfiguracao(entidade, request);
                var atualizado = await _repository.UpdateAsync<ConfiguracaoSms, Guid>(entidade);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(atualizado.Id);
            }
            catch ( Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<ConfiguracaoSmsAutomaticaDTO>>> ObterConfiguracoesAutomaticasAsync(Guid clinicaId)
        {
            try
            {
                var spec = new ConfiguracoesSmsAutomaticasPorClinicaSpec(clinicaId);
                var lista = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(spec);

                var dto = lista.Select(x => new ConfiguracaoSmsAutomaticaDTO
                {
                    Id = x.Id,
                    ClinicaId = x.ClinicaId,
                    Codigo = x.Codigo,
                    Descricao = x.Descricao,
                    Ativo = x.Ativo,
                    Diasantecedencia = x.Diasantecedencia,
                    Textomensagem = x.Textomensagem,
                    TodosMedicos = x.TodosMedicos,
                });

                return ResponseFactory.Success(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ConfiguracaoSmsAutomaticaDTO>>(ex.Message);
            }
        }

        public async Task<Response<ConfiguracaoSmsAutomaticaDTO>> ObterConfiguracaoAutomaticaAsync(Guid clinicaId, string codigo)
        {
            try
            {
                var spec = new ConfiguracaoSmsAutomaticaPorClinicaSpec(clinicaId, codigo);
                var lista = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(spec);
                var entidade = lista.FirstOrDefault();

                if(entidade == null)
                    return ResponseFactory.Fail<ConfiguracaoSmsAutomaticaDTO>("Configuração de SMS automática não encontrada");

                return ResponseFactory.Success(new ConfiguracaoSmsAutomaticaDTO
                {
                    Id = entidade.Id,
                    ClinicaId = entidade.ClinicaId,
                    Codigo = entidade.Codigo,
                    Descricao = entidade.Descricao,
                    Ativo = entidade.Ativo,
                    Diasantecedencia = entidade.Diasantecedencia,
                    Textomensagem = entidade.Textomensagem,
                    TodosMedicos = entidade.TodosMedicos,
                });
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<ConfiguracaoSmsAutomaticaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> GuardarConfiguracaoAutomaticaAsync(Guid clinicaId, AtualizarConfiguracaoAutomaticaRequest request)
        {
            try
            {
                var spec = new ConfiguracaoSmsAutomaticaPorClinicaSpec(clinicaId, request.Codigo);
                var lista = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(spec);
                var entidade = lista.FirstOrDefault();

                if(entidade == null)
                {
                    entidade = new ConfiguracaoSmsAutomatica
                    {
                        ClinicaId = clinicaId,
                        Codigo = request.Codigo,
                    };
                    AplicarConfiguracaoAutomatica(entidade, request);

                    var criado = await _repository.CreateAsync<ConfiguracaoSmsAutomatica, Guid>(entidade);
                    await _repository.SaveChangesAsync();
                    return ResponseFactory.Success(criado.Id);
                }

                AplicarConfiguracaoAutomatica(entidade, request);
                var atualizado = await _repository.UpdateAsync<ConfiguracaoSmsAutomatica, Guid>(entidade);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(atualizado.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }

        }

        public async Task<Response<IEnumerable<string>>> ObterMedicosSelecionadosAsync(Guid clinicaId, string codigoConfiguracao)
        {
            try
            {
                var spec = new ConfiguracaoSmsAutomaticaMedicoSpec(clinicaId, codigoConfiguracao);
                var lista = await _repository.GetListAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(spec);
                return ResponseFactory.Success(lista.Select(x => x.CodigoMedico));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<string>>(ex.Message);
            }
        }

        public async Task<Response<bool>> GuardarMedicosSelecionadosAsync(Guid clinicaId, GuardarMedicosSmsRequest request)
        {
            try
            {
                var specConfig = new ConfiguracaoSmsAutomaticaPorClinicaSpec( clinicaId, request.CodigoConfiguracao);
                var configs = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(specConfig);
                var config = configs.FirstOrDefault();

                if(config == null)
                    return ResponseFactory.Fail<bool>("Configuração de SMS automática não encontrada");

                var spec = new ConfiguracaoSmsAutomaticaMedicoSpec(clinicaId, request.CodigoConfiguracao);
                var atuais = await _repository.GetListAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(spec);
                foreach(var item in atuais)
                    await _repository.RemoveAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(item);

                foreach(var codigo in request.CodigosMedicos.Distinct())
                {
                    if(string.IsNullOrWhiteSpace(codigo)) continue;

                    var novo = new ConfiguracaoSmsAutomaticaMedico
                    {
                        ClinicaId = clinicaId,
                        CodigoConfiguracao = request.CodigoConfiguracao,
                        CodigoMedico = codigo.Trim()
                    };
                    await _repository.CreateAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(novo);
                }

                config.TodosMedicos = false;
                _ = await _repository.UpdateAsync<ConfiguracaoSmsAutomatica, Guid>(config);

                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(true);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<bool>(ex.Message);
            }
        }

        public async Task<Response<bool>> GuardarTodosMedicosAsync(Guid clinicaId, GuardarTodosMedicosSmsRequest request)
        {
            try
            {
                var spec = new ConfiguracaoSmsAutomaticaPorClinicaSpec(clinicaId, request.CodigoConfiguracao);
                var lista = await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(spec);
                var config = lista.FirstOrDefault();

                if(config == null)
                    return ResponseFactory.Fail<bool>("Configuração de SMS automática não encontrada");

                config.TodosMedicos = request.TodosMedicos;
                _ = await _repository.UpdateAsync<ConfiguracaoSmsAutomatica, Guid>(config);

                if(request.TodosMedicos)
                {
                    var specMedicos = new ConfiguracaoSmsAutomaticaMedicoSpec(clinicaId, request.CodigoConfiguracao);
                    var atuais = await _repository.GetListAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(specMedicos);
                    foreach( var item in atuais)
                        await _repository.RemoveAsync<ConfiguracaoSmsAutomaticaMedico, Guid>(item);
                }

                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(true);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<bool>(ex.Message);
            }
        }

        public async Task<PaginatedResponse<HistoricoSmsTabelaDTO>> ObterHistoricoPaginadoAsync(Guid clinicaId, HistoricoSmsTabelaFiltro filtro)
        {
            if(filtro.Filters != null && filtro.Filters.Count > 0)
                filtro.PageNumber = 1;

            var order = filtro.Sorting != null ? GSHelpers.GenerateOrderByString(filtro) : "";
            var spec = new HistoricoSmsTabelaSpec(clinicaId, filtro.Filters ?? [], order);

            return await _repository.GetPaginatedResultsAsync<HistoricoSms, HistoricoSmsTabelaDTO, Guid>(filtro.PageNumber, filtro.PageSize, spec);
        }

        public async Task<Response<Guid>> EnviarSmsTesteAsync(Guid clinicaId, EnviarSmsTesteRequest request)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.NumeroDestinatario))
                    return ResponseFactory.Fail<Guid>("Número de destinatário é obrigatório.");

                if(string.IsNullOrWhiteSpace(request.TextoMensagem))
                    return ResponseFactory.Fail<Guid>("Texto da mensagem é obrigatório.");

                var spec = new ConfiguracaoSmsPorClinicaSpec(clinicaId);
                var configs = await _repository.GetListAsync<ConfiguracaoSms, Guid>(spec);
                var config = configs.FirstOrDefault();

                if(config == null)
                    return ResponseFactory.Fail<Guid>("Configuração de SMS não encontrada.");

                if(!config.Ativo)
                    return ResponseFactory.Fail<Guid>("O serviço de SMS está inativo para a clínica.");

                if(config.UsenditArpoone != 2)
                    return ResponseFactory.Fail<Guid>("Envio manual está disponível apenas para Arpoone nesta versão.");

                if(string.IsNullOrWhiteSpace(config.ArpooneUrl) ||
                   string.IsNullOrWhiteSpace(config.ArpooneSender) ||
                   string.IsNullOrWhiteSpace(config.ArpooneApiKey) ||
                   !config.ArpooneOrganizationID.HasValue)
                {
                    return ResponseFactory.Fail<Guid>("Configuração Arpoone incompleta. Verifica URL, Sender, ApiKey e OrganizationID.");
                }

                var idMensagem = Guid.NewGuid();
                var numeroNormalizado = NormalizarNumeroTelemovel(request.NumeroDestinatario);

                var historico = new HistoricoSms
                {
                    ClinicaId = clinicaId,
                    IdMensagem = idMensagem,
                    TextoMensagem = request.TextoMensagem,
                    NumeroDestinatario = numeroNormalizado,
                    Status = "Pendente",
                    DataHoraCriacao = DateTime.Now,
                    Modulo = string.IsNullOrWhiteSpace(request.Modulo) ? "TesteSMS" : request.Modulo.Trim(),
                    CodigoUtente = request.CodigoUtente,
                    CodigoMedico = string.IsNullOrWhiteSpace(request.CodigoMedico) ? null : request.CodigoMedico.Trim(),
                    CodigoFisioterapeuta = request.CodigoFisioterapeuta,
                    CodigoConsulta = request.CodigoConsulta,
                    CodigoTratamento = request.CodigoTratamento,
                    CodigoAula = request.CodigoAula,
                };

                await _repository.CreateAsync<HistoricoSms, Guid>(historico);
                await _repository.SaveChangesAsync();

                var apiKey = CriptografiaSmsHelper.Decifrar(config.ArpooneApiKey!);
                var payload = new
                {
                    organizationId = config.ArpooneOrganizationID.Value.ToString(),
                    messages = new[]
                    {
                        new
                        {
                            to = numeroNormalizado,
                            text = request.TextoMensagem,
                            from = config.ArpooneSender,
                            expirationDateTime = DateTime.UtcNow.AddHours(24),
                            smsWebhooks = new
                            {
                                delivered = new
                                {
                                    url = config.WebhookDeliveredUrl,
                                    enabled = !string.IsNullOrWhiteSpace(config.WebhookDeliveredUrl),
                                },
                                notDelivered = new
                                {
                                    url = config.WebhookNotDeliveredUrl,
                                    enabled = !string.IsNullOrWhiteSpace(config.WebhookNotDeliveredUrl),
                                },
                                pending = new
                                {
                                    url = config.WebhookPendingUrl,
                                    enabled = !string.IsNullOrWhiteSpace(config.WebhookPendingUrl),
                                },
                                customPayload = idMensagem.ToString(),
                            },
                        },
                    },
                };

                using var httpClient = new HttpClient();
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, config.ArpooneUrl);
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpRequest.Content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                using var response = await httpClient.SendAsync(httpRequest);
                if(response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                {
                    var bodyErro = await response.Content.ReadAsStringAsync();
                    historico.Status = "Não Entregue";
                    historico.MensagemErro = $"Arpoone HTTP {(int)response.StatusCode}: {bodyErro}";
                    _ = await _repository.UpdateAsync<HistoricoSms, Guid>(historico);
                    await _repository.SaveChangesAsync();
                    return ResponseFactory.Fail<Guid>("Falha ao enviar SMS para o provider.");
                }

                historico.Status = "Enviado";
                historico.DataHoraEnvio = DateTime.Now;
                historico.MensagemErro = null;
                _ = await _repository.UpdateAsync<HistoricoSms, Guid>(historico);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success(idMensagem);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        private void AplicarConfiguracao(ConfiguracaoSms entidade, AtualizarConfiguracaoSmsRequest request)
        {
            entidade.Ativo = request.Ativo;
            entidade.UsenditArpoone = request.UsenditArpoone;

            entidade.Url = request.Url;
            entidade.Loginapi = string.IsNullOrWhiteSpace(request.Loginapi) ? string.Empty : CriptografiaSmsHelper.Cifrar(request.Loginapi);
            entidade.Passwordapi = string.IsNullOrWhiteSpace(request.Passwordapi) ? string.Empty : CriptografiaSmsHelper.Cifrar(request.Passwordapi);
            entidade.Numapi = request.Numapi;
            entidade.Remetente = request.Remetente;

            entidade.ArpooneUrl = request.ArpooneUrl;
            entidade.ArpooneSender = request.ArpooneSender;
            entidade.ArpooneApiKey = string.IsNullOrWhiteSpace(request.ArpooneApiKey) ? string.Empty : CriptografiaSmsHelper.Cifrar(request.ArpooneApiKey);
            entidade.ArpooneOrganizationID = request.ArpooneOrganizationID ?? entidade.ArpooneOrganizationID;

            var baseUrl = ObterBaseUrl();
            if(!string.IsNullOrWhiteSpace(baseUrl))
            {
                entidade.WebhookDeliveredUrl = $"{baseUrl}/webhooks/sms/entregue";
                entidade.WebhookNotDeliveredUrl = $"{baseUrl}/webhooks/sms/nao-entregue";
                entidade.WebhookPendingUrl = $"{baseUrl}/webhooks/sms/pendente";
            }
        }

        private string? ObterBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if(request != null && request.Host.HasValue)
                return $"{request.Scheme}://{request.Host}";

            var configurado = _configuration["PublicBaseUrl"] ?? _configuration["App:BaseUrl"];
            if(!string.IsNullOrWhiteSpace(configurado))
                return configurado.TrimEnd('/');

            var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
            if(!string.IsNullOrWhiteSpace(urls))
            {
                var primeira = urls.Split(';', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if(!string.IsNullOrWhiteSpace(primeira))
                    return primeira.TrimEnd('/');
            }

            return null;
        }

        private static void AplicarConfiguracaoAutomatica(ConfiguracaoSmsAutomatica entidade, AtualizarConfiguracaoAutomaticaRequest request)
        {
            entidade.Ativo = request.Ativo;
            entidade.Descricao = request.Descricao;
            entidade.Diasantecedencia = request.Diasantecedencia;
            entidade.Textomensagem = request.Textomensagem;
        }

        private static ConfiguracaoSmsDTO MapToDTO(ConfiguracaoSms entidade, bool decifrarSegredos)
        {
            return new ConfiguracaoSmsDTO
            {
                Id = entidade.Id,
                ClinicaId = entidade.ClinicaId,
                Ativo = entidade.Ativo,
                UsenditArpoone = entidade.UsenditArpoone,
                
                Url = entidade.Url,
                Loginapi = decifrarSegredos && !string.IsNullOrWhiteSpace(entidade.Loginapi)
                    ? CriptografiaSmsHelper.Decifrar(entidade.Loginapi)
                    : entidade.Loginapi,
                Passwordapi = decifrarSegredos && !string.IsNullOrWhiteSpace(entidade.Passwordapi)
                    ? CriptografiaSmsHelper.Decifrar(entidade.Passwordapi)
                    : entidade.Passwordapi,
                Numapi = entidade.Numapi,
                Remetente = entidade.Remetente,

                ArpooneUrl = entidade.ArpooneUrl, 
                ArpooneSender = entidade.ArpooneSender,
                ArpooneApiKey = decifrarSegredos && !string.IsNullOrWhiteSpace(entidade.ArpooneApiKey)
                    ? CriptografiaSmsHelper.Decifrar(entidade.ArpooneApiKey)
                    : entidade.ArpooneApiKey,
                ArpooneOrganizationID = entidade.ArpooneOrganizationID,

                WebhookDeliveredUrl = entidade.WebhookDeliveredUrl,
                WebhookNotDeliveredUrl = entidade.WebhookNotDeliveredUrl,
                WebhookPendingUrl = entidade.WebhookPendingUrl,

                ControloSmsAutomaticos = entidade.ControloSmsAutomaticos,
            };
        }

        private static string NormalizarNumeroTelemovel(string numero)
        {
            var apenasDigitos = new string((numero ?? string.Empty).Where(char.IsDigit).ToArray());
            if(string.IsNullOrWhiteSpace(apenasDigitos))
                return string.Empty;

            if(apenasDigitos.StartsWith("351", StringComparison.Ordinal) && apenasDigitos.Length == 12)
                return apenasDigitos;

            if(apenasDigitos.StartsWith('0') && apenasDigitos.Length == 10)
                return $"351{apenasDigitos[1..]}";

            if(apenasDigitos.Length == 9)
                return $"351{apenasDigitos}";

            return apenasDigitos;
        }

        public async Task<Response<Guid>> EnviarSmsPorCodigoAsync(Guid clinicaId, EnviarSmsPorCodigoRequest request)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.CodigoConfiguracao))
                    return ResponseFactory.Fail<Guid>("Código de configuração é obrigatório.");

                if(string.IsNullOrWhiteSpace(request.NumeroDestinatario))
                    return ResponseFactory.Fail<Guid>("Número de destinatário é obrigatório.");

                var cfgSpec = new ConfiguracaoSmsPorClinicaSpec(clinicaId);
                var cfg = (await _repository.GetListAsync<ConfiguracaoSms, Guid>(cfgSpec)).FirstOrDefault();
                if(cfg == null)
                    return ResponseFactory.Fail<Guid>("Configuração de SMS não encontrada.");

                if(!cfg.Ativo)
                    return ResponseFactory.Fail<Guid>("O serviço de SMS está inativo para a clínica.");

                if(cfg.UsenditArpoone != 2)
                    return ResponseFactory.Fail<Guid>("Envio manual está disponível apenas para Arpoone nesta versão.");

                if(string.IsNullOrWhiteSpace(cfg.ArpooneUrl) || 
                   string.IsNullOrWhiteSpace(cfg.ArpooneSender) ||
                   string.IsNullOrWhiteSpace(cfg.ArpooneApiKey) ||
                   !cfg.ArpooneOrganizationID.HasValue)
                {
                    return ResponseFactory.Fail<Guid>("Configuração Arpoone incompleta. Verifica URL, Sender, ApiKey e OrganizationID.");
                }

                var regSpec = new ConfiguracaoSmsAutomaticaPorClinicaSpec(clinicaId, request.CodigoConfiguracao);
                var reg = (await _repository.GetListAsync<ConfiguracaoSmsAutomatica, Guid>(regSpec)).FirstOrDefault();

                if(reg == null)
                    return ResponseFactory.Fail<Guid>("Configuração de SMS automática não encontrada para o código indicado.");
                
                if(reg.Ativo != 1)
                    return ResponseFactory.Fail<Guid>("A configuração de SMS automática está desativada.");

                var template = (reg.Textomensagem ?? string.Empty).Trim();
                if(string.IsNullOrWhiteSpace(template))
                    return ResponseFactory.Fail<Guid>($"Template de SMS vazio para o codigo {request.CodigoConfiguracao}");

                var placeholders = ConstruirPlaceholdersEnvioPorCodigo(request.CodigoConfiguracao, request);
                var textoMensagem = SmsTemplateRenderer.Render(template, placeholders);

                if(string.IsNullOrWhiteSpace(textoMensagem))
                    return ResponseFactory.Fail<Guid>("Mensagem final vazia após substituir placeholders.");

                return await EnviarSmsTesteAsync(clinicaId, new EnviarSmsTesteRequest
                {
                    NumeroDestinatario = request.NumeroDestinatario, 
                    TextoMensagem = textoMensagem,
                    Modulo = string.IsNullOrWhiteSpace(request.Modulo) ? $"SMSCodigo-{request.CodigoConfiguracao}" : request.Modulo,
                    CodigoUtente = request.CodigoUtente,
                    CodigoMedico = request.CodigoMedico,
                    CodigoFisioterapeuta = request.CodigoFisioterapeuta,
                    CodigoConsulta = request.CodigoConsulta,
                    CodigoTratamento = request.CodigoTratamento, 
                    CodigoAula = request.CodigoAula,
                });
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        private static Dictionary<string, string> ConstruirPlaceholdersEnvioPorCodigo(string codigoConfiguracao,EnviarSmsPorCodigoRequest request)
        {
            static string FDate(DateTime? d) => d?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
            static string NZ(string? s) => s?.Trim() ?? string.Empty;


            var data = FDate(request.Data);
            var hora = NZ(request.Hora);

            var dataAntiga = FDate(request.DataAntiga);
            var horaAntiga = NZ(request.HoraAntiga);

            var dataNova = FDate(request.DataNova);
            var horaNova = NZ(request.HoraNova);

            var medico = NZ(request.NomeMedico);
            if(string.IsNullOrWhiteSpace(medico)) medico = NZ(request.NomeMedicoOuProfissional);

            var fisioterapeuta = NZ(request.NomeFisioterapeuta);
            if(string.IsNullOrWhiteSpace(fisioterapeuta)) fisioterapeuta = NZ(request.NomeMedicoOuProfissional);

            var profissional = NZ(request.NomeProfissional);
            if(string.IsNullOrWhiteSpace(profissional)) profissional = NZ(request.NomeMedicoOuProfissional);

            var modalidade = NZ(request.NomeModalidade);
            if(string.IsNullOrWhiteSpace(modalidade)) modalidade = NZ(request.NomeEspecialidade);

            var especialidade = NZ(request.NomeEspecialidade);
            var nSessao = NZ(request.NumeroSessao);
            var utente = NZ(request.NomeUtente);

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Utente"] = utente,

                ["Data"] = data,
                ["Hora"] = hora,

                ["DataAntiga"] = dataAntiga,
                ["HoraAntiga"] = horaAntiga,

                ["DataNova"] = dataNova,
                ["HoraNova"] = horaNova,

                ["Medico"] = medico,
                ["Fisioterapeuta"] = fisioterapeuta, 
                ["Profissional"] = profissional,

                ["Especialidade"] = especialidade,
                ["Modalidade"] = modalidade,

                ["Nsessao"] = nSessao,
                ["NSessao"] = nSessao,

            };

            switch((codigoConfiguracao ?? string.Empty).Trim())
            {
                case "3": 
                    map["Data"] = string.Empty;
                    map["Hora"] = string.Empty;
                    break;

                case "4":
                    if(string.IsNullOrWhiteSpace(map["DataAntiga"])) map["DataAntiga"] = map["Data"];
                    if(string.IsNullOrWhiteSpace(map["HoraAntiga"])) map["HoraAntiga"] = map["Hora"];
                    if(string.IsNullOrWhiteSpace(map["DataNova"])) map["DataNova"] = map["Data"];
                    if(string.IsNullOrWhiteSpace(map["HoraNova"])) map["HoraNova"] = map["Hora"];
                    break;

                default: 
                    break;
            }

            return map;
        }
    }
 
}
