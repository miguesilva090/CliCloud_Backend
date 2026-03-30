using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.Sms
{
    [Route("webhooks/sms")]
    [ApiController]
    [AllowAnonymous]

    public class SmsWebhookController( IServicoWebhookSms servicoWebhookSms) : ControllerBase
    {
        private readonly IServicoWebhookSms _servicoWebhookSms = servicoWebhookSms;

        [HttpPost("entregue")]
        public async Task<IActionResult> EntregueAsync([FromBody] List<WebhookEstadoSmsDTO> payload)
        {
            await _servicoWebhookSms.ProcessarEntregueAsync(payload);
            return Ok(new { success = true });
        }

        [HttpPost("nao-entregue")]
        public async Task<IActionResult> NaoEntregueAsync([FromBody] List<WebhookEstadoSmsDTO> payload)
        {
            await _servicoWebhookSms.ProcessarNaoEntregueAsync(payload ?? []);
            return Ok( new { success = true });
        }

        [HttpPost("pendente")]
        public async Task<IActionResult> PendenteAsync([FromBody] List<WebhookEstadoSmsDTO> payload)
        {
            await _servicoWebhookSms.ProcessarPendenteAsync(payload ?? []);
            return Ok( new { success = true });
        }

        [HttpPost("inbound")]
        public async Task<IActionResult> InboundAsync([FromBody] List<WebhookInboundSmsDTO> payload)
        {
            await _servicoWebhookSms.ProcessarInboundAsync(payload ?? []);
            return Ok( new { success = true });
        }
    }
}