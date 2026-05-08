using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.TipoCartaService;
using CliCloud.Application.Services.Utility.TipoCartaService.DTOs;
using CliCloud.Application.Services.Utility.TipoCartaService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Utility
{
  [Route("client/utility/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class TipoCartaController(ITipoCartaService service) : ControllerBase
  {
    private readonly ITipoCartaService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(string keyword = "")
    {
      Response<IEnumerable<TipoCartaDTO>> result = await _service.GetAllAsync(keyword);
      return Ok(result);
    }

    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync([FromBody] TipoCartaTableFilter filter)
    {
      PaginatedResponse<TipoCartaTableDTO> result = await _service.GetPaginatedAsync(filter);
      return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<TipoCartaDTO> result = await _service.GetByIdAsync(id);
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTipoCartaRequest request)
    {
      Response<Guid> result = await _service.CreateAsync(request);
      return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateTipoCartaRequest request, Guid id)
    {
      Response<Guid> result = await _service.UpdateAsync(request, id);
      return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _service.DeleteAsync(id);
      return Ok(result);
    }

    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleTipoCartaRequest request)
    {
      Response<IEnumerable<Guid>> result = await _service.DeleteMultipleAsync(request.Ids);
      return Ok(result);
    }
  }
}
