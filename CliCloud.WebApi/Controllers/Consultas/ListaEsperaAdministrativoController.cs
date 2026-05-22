using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/lista-espera-administrativo")]
[ApiController]
public class ListaEsperaAdministrativoController(IListaEsperaAdministrativoService service)
  : ControllerBase
{
  private readonly IListaEsperaAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] ListaEsperaTableFilter filter)
    => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
    => Ok(await _service.GetByIdAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateListaEsperaRequest request)
    => Ok(await _service.CreateAsync(request));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListaEsperaRequest request)
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
    [FromBody] AppendListaEsperaObservacaoRequest request
  )
    => Ok(await _service.AppendObservacaoAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/converter-marcacao")]
  public async Task<IActionResult> ConverterParaMarcacao(
    Guid id,
    [FromBody] ConverterListaEsperaMarcacaoRequest request
  )
    => Ok(await _service.ConverterParaMarcacaoAsync(id, request));
}
