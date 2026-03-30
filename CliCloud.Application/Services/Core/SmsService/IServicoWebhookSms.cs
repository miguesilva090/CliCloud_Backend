using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Core.SmsService.DTOs;

namespace CliCloud.Application.Services.Core.SmsService
{
    public interface IServicoWebhookSms : ITransientService
    {
        Task ProcessarEntregueAsync(IEnumerable<WebhookEstadoSmsDTO> payload);
        Task ProcessarNaoEntregueAsync(IEnumerable<WebhookEstadoSmsDTO> payload);
        Task ProcessarPendenteAsync(IEnumerable<WebhookEstadoSmsDTO> payload);
        Task ProcessarInboundAsync(IEnumerable<WebhookInboundSmsDTO> payload);
    }
}
