using CliCloud.Application.Services.Faturacao.AdseComunicacaoService;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/adse/comunicacao")]
[ApiController]
[Authorize(Roles = "client")]
public class AdseComunicacaoController(IAdseComunicacaoService service) : ControllerBase
{
    [HttpPost("{modulo}/paginated")]
    public async Task<IActionResult> GetPaginated(string modulo, [FromBody] AdseComunicacaoTableFilter filter)
    {
        filter.Modulo = modulo;
        return Ok(await service.GetPaginatedAsync(modulo, filter));
    }

    [HttpGet("pre-faturas/abertas")]
    public async Task<IActionResult> PreFaturasAbertas([FromQuery] string tipoPreFatura)
        => Ok(await service.ListarPreFaturasAbertasAsync(tipoPreFatura));

    [HttpGet("pre-faturas")]
    public async Task<IActionResult> PreFaturasPorEstado([FromQuery] string tipoPreFatura, [FromQuery] int estado)
        => Ok(await service.ListarPreFaturasPorEstadoAsync(tipoPreFatura, estado));

    [HttpPost("pre-faturas")]
    public async Task<IActionResult> CriarPreFatura([FromBody] CriarAdsePreFaturaRequest request)
        => Ok(await service.CriarPreFaturaAsync(request));

    [HttpDelete("pre-faturas/{id:guid}")]
    public async Task<IActionResult> ApagarPreFatura(Guid id, CancellationToken ct)
        => Ok(await service.ApagarPreFaturaAsync(id, ct));

    [HttpGet("pre-faturas/{id:guid}")]
    public async Task<IActionResult> ConsultarPreFatura(Guid id, CancellationToken ct)
        => Ok(await service.ConsultarPreFaturaAsync(id, ct));

    [HttpPost("pre-faturas/{id:guid}/conferir")]
    public async Task<IActionResult> ConferirPreFatura(Guid id, CancellationToken ct)
        => Ok(await service.ConferirPreFaturaAsync(id, ct));

    [HttpPost("pre-faturas/{id:guid}/fechar")]
    public async Task<IActionResult> FecharPreFatura(Guid id, [FromBody] AdseFecharPreFaturaRequest request, CancellationToken ct)
        => Ok(await service.FecharPreFaturaAsync(id, request, ct));

    [HttpPost("{modulo}/pdf")]
    public async Task<IActionResult> Pdf(string modulo, [FromBody] AdseUploadPdfRequest request, CancellationToken ct)
    {
        if (!AdseComunicacaoModulo.TryParse(modulo, out string tipo))
            return BadRequest("Módulo inválido.");
        return Ok(await service.RegistarPdfAsync(request, tipo, ct));
    }

    [HttpPost("documentos/comunicar")]
    public async Task<IActionResult> Comunicar([FromBody] AdseComunicarDocumentosRequest request, CancellationToken ct)
        => Ok(await service.ComunicarDocumentosAsync(request, ct));

    [HttpPost("documentos/libertar")]
    public async Task<IActionResult> Libertar([FromBody] AdseLibertarDocumentosRequest request, CancellationToken ct)
        => Ok(await service.LibertarDocumentosAsync(request, ct));
}
