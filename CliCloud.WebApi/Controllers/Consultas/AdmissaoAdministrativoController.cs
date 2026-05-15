using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/admissoes-administrativo")]
[ApiController]
public class AdmissaoAdministrativoController(IAdmissaoAdministrativoService service) : ControllerBase
{
  private readonly IAdmissaoAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] AdmissaoTableFilter filter)
    => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
    => Ok(await _service.GetByIdAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateAdmissaoRequest request)
    => Ok(await _service.CreateAsync(request));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAdmissaoRequest request)
    => Ok(await _service.UpdateAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
    => Ok(await _service.DeleteAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/confirmar")]
  public async Task<IActionResult> Confirmar(Guid id, [FromBody] bool confirmado)
    => Ok(await _service.ConfirmarAsync(id, confirmado));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/efetuado")]
  public async Task<IActionResult> SetEfetuado(Guid id, [FromBody] bool efetuado)
    => Ok(await _service.SetEfetuadoAsync(id, efetuado));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/promover-consulta")]
  public async Task<IActionResult> PromoverParaConsulta(Guid id)
    => Ok(await _service.PromoverParaConsultaAsync(id));
}
