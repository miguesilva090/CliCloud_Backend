using CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService;
using CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/marcacao-automatica-tratamento")]
[ApiController]
public class MarcacaoAutomaticaTratamentoController(
    IMarcacaoAutomaticaTratamentoService service
) : ControllerBase
{
    private readonly IMarcacaoAutomaticaTratamentoService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost("preview")]
    public async Task<IActionResult> Preview([FromBody] MarcacaoAutomaticaPreviewRequest request)
        => Ok(await _service.PreviewAsync(request));

    [Authorize(Roles = "client")]
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] MarcacaoAutomaticaConfirmRequest request)
        => Ok(await _service.ConfirmAsync(request));

}