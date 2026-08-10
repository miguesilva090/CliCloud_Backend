using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/historico-tratamento-administrativo")]
[ApiController]
public class HistoricoTratamentoAdministrativoController(
  IHistoricoTratamentoAdministrativoService service
) : ControllerBase
{
  private readonly IHistoricoTratamentoAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated(
    [FromBody] HistoricoTratamentoTableFilter filter
  ) => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/passar-historico")]
  public async Task<IActionResult> PassarParaHistorico(Guid id)
    => Ok(await _service.PassarParaHistoricoAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/reabrir")]
  public async Task<IActionResult> Reabrir(Guid id)
    => Ok(await _service.ReabrirAsync(id));

  [Authorize(Roles = "client")]
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
    => Ok(await _service.DeleteAsync(id));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}/observacoes")]
  public async Task<IActionResult> GetObservacoes(Guid id)
    => Ok(await _service.GetObservacoesAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/observacoes")]
  public async Task<IActionResult> AppendObservacao(
    Guid id,
    [FromBody] AppendHistoricoTratamentoObservacaoRequest request
  ) => Ok(await _service.AppendObservacaoAsync(id, request));
}
