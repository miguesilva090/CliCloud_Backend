using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/fecho-diario-administrativo")]
[ApiController]
public class FechoDiarioTratamentoAdministrativoController(
  IFechoDiarioTratamentoAdministrativoService service
) : ControllerBase
{
  private readonly IFechoDiarioTratamentoAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpGet("contagem")]
  public async Task<IActionResult> Contar([FromQuery] DateTime data) =>
    Ok(await _service.ContarElegiveisAsync(data.Date));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Executar([FromBody] FechoDiarioTratamentoRequest request) =>
    Ok(await _service.ExecutarFechoAsync(request));
}