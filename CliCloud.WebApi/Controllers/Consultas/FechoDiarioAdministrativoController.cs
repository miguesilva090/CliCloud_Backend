using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/fecho-diario-administrativo")]
[ApiController]
public class FechoDiarioAdministrativoController(IFechoDiarioAdministrativoService service) : ControllerBase
{
  private readonly IFechoDiarioAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Executar([FromBody] FechoDiarioRequest request)
    => Ok(await _service.ExecutarFechoAsync(request));
}
