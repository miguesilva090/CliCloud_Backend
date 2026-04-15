using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class SeparadorPersonalizadoDocumentoController(
        ISeparadorPersonalizadoDocumentoService service
    ) : ControllerBase
    {
        private readonly ISeparadorPersonalizadoDocumentoService _service = service;

        [Authorize(Roles = "client")]
        [HttpGet("modelo/{separadorId:guid}")]
        public async Task<IActionResult> GetModeloAsync(Guid separadorId)
        {
            return Ok(await _service.GetModeloAsync(separadorId));
        }

        [Authorize(Roles = "client")]
        [HttpPost("modelo")]
        public async Task<IActionResult> UpsertModeloAsync(
            [FromBody] UpsertSeparadorPersonalizadoModeloRequest request
        )
        {
            return Ok(await _service.UpsertModeloAsync(request));
        }

        [Authorize(Roles = "client")]
        [HttpPost("impressao")]
        public async Task<IActionResult> GerarImpressaoAsync(
            [FromBody] GerarImpressaoSeparadorPersonalizadoRequest request
        )
        {
            return Ok(await _service.GerarImpressaoAsync(request));
        }
    }
}
