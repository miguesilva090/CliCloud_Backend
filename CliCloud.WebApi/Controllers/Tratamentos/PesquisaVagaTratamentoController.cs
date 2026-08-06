using CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService;
using CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/pesquisa-vaga-tratamento")]
[ApiController]
public class PesquisaVagaTratamentoController(
    IPesquisaVagaTratamentoService service
) : ControllerBase
{
    private readonly IPesquisaVagaTratamentoService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost("pesquisar")]
    public async Task<IActionResult> Pesquisar([FromBody] PesquisaVagaRequest request)
        => Ok(await _service.PesquisarAsync(request));
}