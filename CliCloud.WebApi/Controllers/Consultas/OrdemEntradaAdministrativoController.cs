using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/ordem-entrada-administrativo")]
[ApiController]
public class OrdemEntradaAdministrativoController(IOrdemEntradaAdministrativoService service)
  : ControllerBase
{
  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] OrdemEntradaTableFilter filter)
    => Ok(await service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetRegisto(Guid id)
    => Ok(await service.GetRegistoAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> CreateRegisto([FromBody] SaveOrdemEntradaRegistoRequest request)
    => Ok(await service.CreateRegistoAsync(request));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpdateRegisto(
    Guid id,
    [FromBody] SaveOrdemEntradaRegistoRequest request
  )
    => Ok(await service.UpdateRegistoAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/anular")]
  public async Task<IActionResult> Anular(Guid id, [FromBody] AnularOrdemEntradaRequest request)
    => Ok(await service.AnularAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("horas-disponiveis")]
  public async Task<IActionResult> GetHorasDisponiveis(
    [FromBody] OrdemEntradaHorasDisponiveisRequest request
  )
    => Ok(await service.GetHorasDisponiveisAsync(request));
}
