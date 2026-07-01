using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.DTOs;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/configuracao-adse")]
[ApiController]
[Authorize(Roles = "client")]
public class ConfiguracaoADSEController(
  IConfiguracaoADSEService service,
  ICurrentClinicaService currentClinicaService
) : ControllerBase
{
  private readonly IConfiguracaoADSEService _service = service;
  private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

  private async Task<Guid?> ObterEmpresaIdAsync(Guid? empresaId = null)
  {
    if (empresaId is Guid id && id != Guid.Empty)
      return id;

    await _currentClinicaService.SetClinicaAsync();
    return Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) ? clinicaId : null;
  }

  private static IActionResult ToActionResult<T>(Response<T> result)
  {
    return result.Status == ResponseStatus.Success
      ? new OkObjectResult(result)
      : new BadRequestObjectResult(result);
  }

  [HttpGet("configuracao")]
  public async Task<IActionResult> ObterConfiguracaoPorEmpresaAsync([FromQuery] Guid? empresaId = null)
  {
    Guid? resolvedEmpresaId = await ObterEmpresaIdAsync(empresaId);
    if (resolvedEmpresaId is null)
      return BadRequest("Empresa inválida.");

    Response<ConfiguracaoADSEDTO> result = await _service.ObterConfiguracaoPorEmpresaAsync(resolvedEmpresaId.Value);
    return ToActionResult(result);
  }

  [HttpPut("configuracao")]
  [HttpPost("configuracao")]
  public async Task<IActionResult> GuardarConfiguracaoAsync(
    [FromBody] GuardarConfiguracaoADSERequest request,
    [FromQuery] Guid? empresaId = null)
  {
    Guid? resolvedEmpresaId = await ObterEmpresaIdAsync(empresaId ?? request.EmpresaId);
    if (resolvedEmpresaId is null)
      return BadRequest("Empresa inválida.");

    Response<Guid> result = await _service.GuardarConfiguracaoAsync(resolvedEmpresaId.Value, request);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetConfiguracaoADSEAsync(string keyword = "")
  {
    Response<IEnumerable<ConfiguracaoADSEDTO>> result = await _service.GetConfiguracaoADSEAsync(keyword);
    return ToActionResult(result);
  }

  [HttpPost("paginated")]
  public async Task<IActionResult> GetConfiguracaoADSEPaginatedAsync([FromBody] ConfiguracaoADSETableFilter filter)
  {
    PaginatedResponse<ConfiguracaoADSEDTO> result = await _service.GetConfiguracaoADSEPaginatedAsync(filter);
    return Ok(result);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetConfiguracaoADSEByIdAsync(Guid id)
  {
    Response<ConfiguracaoADSEDTO> result = await _service.GetConfiguracaoADSEAsync(id);
    return ToActionResult(result);
  }

  [HttpPost]
  public async Task<IActionResult> CreateConfiguracaoADSEAsync([FromBody] CreateConfiguracaoADSERequest request)
  {
    Response<Guid> result = await _service.CreateConfiguracaoADSEAsync(request);
    return ToActionResult(result);
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpdateConfiguracaoADSEAsync(
    [FromRoute] Guid id,
    [FromBody] UpdateConfiguracaoADSERequest request,
    [FromQuery] Guid? empresaId = null)
  {
    if (request.EmpresaId == Guid.Empty)
    {
      Guid? resolvedEmpresaId = await ObterEmpresaIdAsync(empresaId);
      if (resolvedEmpresaId is null)
        return BadRequest("Empresa inválida.");
      request.EmpresaId = resolvedEmpresaId.Value;
    }

    Response<Guid> result = await _service.UpdateConfiguracaoADSEAsync(request, id);
    return ToActionResult(result);
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteConfiguracaoADSEAsync(Guid id)
  {
    Response<Guid> result = await _service.DeleteConfiguracaoADSEAsync(id);
    return ToActionResult(result);
  }
}
