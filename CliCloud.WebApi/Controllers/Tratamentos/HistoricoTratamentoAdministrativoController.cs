using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;
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
}
