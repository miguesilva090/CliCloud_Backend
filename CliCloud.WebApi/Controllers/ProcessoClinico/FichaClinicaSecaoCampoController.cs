using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico
{
  [Route("client/processo-clinico/[controller]")]
  [ApiController]
  public class FichaClinicaSecaoCampoController(IFichaClinicaSecaoCampoService service)
    : ControllerBase
  {
    private readonly IFichaClinicaSecaoCampoService _service = service;

    // Lista simples por separador (para combos, etc.)
    [Authorize(Roles = "client")]
    [HttpGet("por-separador/{separadorId:guid}")]
    public async Task<IActionResult> GetBySeparadorAsync(
      Guid separadorId,
      [FromQuery] string keyword = ""
    )
    {
      Response<IEnumerable<FichaClinicaSecaoCampoDTO>> result =
        await _service.GetFichaClinicaSecaoCampoAsync(separadorId, keyword);
      return Ok(result);
    }

    // Tabela paginada (TanStack Table)
    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(
      [FromBody] FichaClinicaSecaoCampoTableFilter filter
    )
    {
      PaginatedResponse<FichaClinicaSecaoCampoDTO> result =
        await _service.GetFichaClinicaSecaoCampoPaginatedAsync(filter);
      return Ok(result);
    }

    // Detalhe por Id
    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<FichaClinicaSecaoCampoDTO> result =
        await _service.GetFichaClinicaSecaoCampoAsync(id);
      return Ok(result);
    }

    // Criar
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateFichaClinicaSecaoCampoRequest request
    )
    {
      Response<Guid> result = await _service.CreateFichaClinicaSecaoCampoAsync(request);
      return Ok(result);
    }

    // Atualizar
    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
      [FromBody] UpdateFichaClinicaSecaoCampoRequest request,
      Guid id
    )
    {
      Response<Guid> result = await _service.UpdateFichaClinicaSecaoCampoAsync(request, id);
      return Ok(result);
    }

    // Apagar único
    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _service.DeleteFichaClinicaSecaoCampoAsync(id);
      return Ok(result);
    }

    // Apagar múltiplos (bulk)
    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] IEnumerable<Guid> ids)
    {
      Response<IEnumerable<Guid>> result =
        await _service.DeleteMultipleFichaClinicaSecaoCampoAsync(ids);
      return Ok(result);
    }
  }
}

