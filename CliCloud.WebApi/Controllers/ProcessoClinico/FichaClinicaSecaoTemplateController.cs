using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico
{
  [Route("client/processo-clinico/[controller]")]
  [ApiController]
  public class FichaClinicaSecaoTemplateController(IFichaClinicaSecaoTemplateService service)
    : ControllerBase
  {
    private readonly IFichaClinicaSecaoTemplateService _service = service;

    // Lista simples (para combos, etc.)
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string keyword = "")
    {
      Response<IEnumerable<FichaClinicaSecaoTemplateDTO>> result =
        await _service.GetFichaClinicaSecaoTemplateAsync(keyword);
      return Ok(result);
    }

    // Tabela paginada (TanStack Table)
    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(
      [FromBody] FichaClinicaSecaoTemplateTableFilter filter
    )
    {
      PaginatedResponse<FichaClinicaSecaoTemplateDTO> result =
        await _service.GetFichaClinicaSecaoTemplatePaginatedAsync(filter);
      return Ok(result);
    }

    // Detalhe por Id
    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<FichaClinicaSecaoTemplateDTO> result =
        await _service.GetFichaClinicaSecaoTemplateAsync(id);
      return Ok(result);
    }

    // Criar
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateFichaClinicaSecaoTemplateRequest request
    )
    {
      Response<Guid> result = await _service.CreateFichaClinicaSecaoTemplateAsync(request);
      return Ok(result);
    }

    // Atualizar
    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
      [FromBody] UpdateFichaClinicaSecaoTemplateRequest request,
      Guid id
    )
    {
      Response<Guid> result = await _service.UpdateFichaClinicaSecaoTemplateAsync(request, id);
      return Ok(result);
    }

    // Apagar único
    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _service.DeleteFichaClinicaSecaoTemplateAsync(id);
      return Ok(result);
    }

    // Apagar múltiplos (bulk)
    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] IEnumerable<Guid> ids)
    {
      Response<IEnumerable<Guid>> result =
        await _service.DeleteMultipleFichaClinicaSecaoTemplateAsync(ids);
      return Ok(result);
    }
  }
}

