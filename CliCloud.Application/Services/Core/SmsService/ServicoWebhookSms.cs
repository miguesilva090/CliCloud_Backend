using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Specifications;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService
{
    public class ServicoWebhookSms(IRepositoryAsync repository) : IServicoWebhookSms
    {
        private readonly IRepositoryAsync _repository = repository;

        public async Task ProcessarEntregueAsync(IEnumerable<WebhookEstadoSmsDTO> payload)
        => await ProcessarEstadoAsync(payload, "Entregue", true);

        public async Task ProcessarNaoEntregueAsync( IEnumerable<WebhookEstadoSmsDTO> payload)
        => await ProcessarEstadoAsync(payload, "Não Entregue", true);

        public async Task ProcessarPendenteAsync( IEnumerable<WebhookEstadoSmsDTO> payload)
        => await ProcessarEstadoAsync(payload, "Pendente", false);

        public async Task ProcessarInboundAsync( IEnumerable<WebhookInboundSmsDTO> payload)
        {
            foreach( var item in payload)
            {
                Guid? organizationId = null;
                if(!string.IsNullOrWhiteSpace(item.OrganizationId)
                    && Guid.TryParse(item.OrganizationId, out var orgId))
                {
                    organizationId = orgId;
                }

                var entidade = new SmsRecebido
                {
                    ClinicaId = organizationId ?? Guid.Empty,
                    OrganizacaoId = organizationId,
                    DataHoraRecebimento = item.ReceiveDateTime,
                    NumeroOrigem = item.From ?? string.Empty,
                    NumeroDestino = item.To ?? string.Empty,
                    Keyword = item.Keyword ?? string.Empty,
                    TextoMensagem = item.Text ?? string.Empty,
                    Encoding = item.Encoding,
                    Mcc = item.Mcc ?? string.Empty,
                    Mnc = item.Mnc ?? string.Empty,
                    TotalSegmentos = item.TotalSegments,
                    DataHoraProcessamento = DateTime.Now
                };

                await _repository.CreateAsync<SmsRecebido, Guid>(entidade);
            }

            await _repository.SaveChangesAsync();
        }

        private async Task ProcessarEstadoAsync( IEnumerable<WebhookEstadoSmsDTO> payload, string estadoFinal, bool permitirSobrescrever)
        {
            foreach( var item in payload)
            {
                if(string.IsNullOrWhiteSpace(item.CustomPayload))
                    continue;
                
                if(!Guid.TryParse(item.CustomPayload, out var idMensagem))
                    continue;

                var spec = new HistoricoSmsPorIdMensagemSpec(idMensagem);
                var lista = await _repository.GetListAsync<HistoricoSms, Guid>(spec);
                var historico = lista.FirstOrDefault();
                if(historico == null) continue;

                if(!permitirSobrescrever)
                {
                    if(!string.Equals(historico.Status, "Pendente", StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                historico.Status = estadoFinal;
                historico.DataHoraEnvio = item.ReportDateTime ?? DateTime.Now;
                historico.MensagemErro = string.IsNullOrWhiteSpace(item.ReportDescription)
                    ? historico.MensagemErro
                    : item.ReportDescription;

                _ = await _repository.UpdateAsync<HistoricoSms, Guid>(historico);
            }
            await _repository.SaveChangesAsync();
        }
    }
}