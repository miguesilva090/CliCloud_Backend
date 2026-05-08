using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.SalaService;
using CliCloud.Application.Services.Consultas.SalaService.DTOs;
using CliCloud.Application.Services.Consultas.SalaService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas
{
  [Route("client/consultas/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class SalaController(ISalaService service) : ControllerBase
  {
    private readonly ISalaService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(string keyword = "")
    {
      Response<IEnumerable<SalaDTO>> result = await _service.GetAllAsync(keyword);
      return Ok(result);
    }

    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync([FromBody] SalaTableFilter filter)
    {
      PaginatedResponse<SalaTableDTO> result = await _service.GetPaginatedAsync(filter);
      return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<SalaDTO> result = await _service.GetByIdAsync(id);
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSalaRequest request)
    {
      Response<Guid> result = await _service.CreateAsync(request);
      return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateSalaRequest request, Guid id)
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
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleSalaRequest request)
    {
      Response<IEnumerable<Guid>> result = await _service.DeleteMultipleAsync(request.Ids);
      return Ok(result);
    }
  }
}
