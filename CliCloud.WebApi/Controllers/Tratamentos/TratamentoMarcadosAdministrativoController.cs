using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/tratamento-marcados-administrativo")]
[ApiController]
public class TratamentoMarcadosAdministrativoController(
  ITratamentoMarcadosAdministrativoService service
) : ControllerBase
{
  private readonly ITratamentoMarcadosAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated(
    [FromBody] TratamentoMarcadosTableFilter filter
  ) => Ok(await _service.GetPaginatedAsync(filter));
}
