using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/lista-espera-tratamento-administrativo")]
[ApiController]
public class ListaEsperaTratamentoAdministrativoController(
    IListaEsperaTratamentoAdministrativoService service
) : ControllerBase
{
    private readonly IListaEsperaTratamentoAdministrativoService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginated(
        [FromBody] ListaEsperaTratamentoTableFilter filter
    )
        => Ok(await _service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListaEsperaTratamentoRequest request)
        => Ok(await _service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateListaEsperaTratamentoRequest request
    )
        => Ok(await _service.UpdateAsync(id, request));

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost("delete-multiple")]
    public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<Guid> ids)
        => Ok(await _service.DeleteMultipleAsync(ids));

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}/observacoes")]
    public async Task<IActionResult> GetObservacoes(Guid id)
        => Ok(await _service.GetObservacoesAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost("{id:guid}/observacoes")]
    public async Task<IActionResult> AppendObservacao(
        Guid id,
        [FromBody] AppendListaEsperaTratamentoObservacaoRequest request
    )
        => Ok(await _service.AppendObservacaoAsync(id, request));

    [Authorize(Roles = "client")]
    [HttpGet("proximo-identificador")]
    public async Task<IActionResult> GetProximoIdentificador()
        => Ok(await _service.GetProximoIdentificadorAsync());

    [Authorize(Roles = "client")]
    [HttpGet("verificar-ordem/{ordem:int}")]
    public async Task<IActionResult> VerificarOrdem(int ordem, [FromQuery] Guid? excludeId = null)
        => Ok(await _service.VerificarOrdemDisponivelAsync(ordem, excludeId));
}
