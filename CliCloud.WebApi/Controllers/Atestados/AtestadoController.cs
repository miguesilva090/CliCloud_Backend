using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Atestados.AtestadoService;
using CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using CliCloud.Application.Services.Atestados.AtestadoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Atestados
{
  [Route("client/atestados/[controller]")]
  [ApiController]
  public class AtestadoController(IAtestadoService atestadoService) : ControllerBase
  {
    private readonly IAtestadoService _atestadoService = atestadoService;

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetAtestadoPaginatedAsync(AtestadoTableFilter filter)
    {
      PaginatedResponse<AtestadoTableDTO> result = await _atestadoService.GetAtestadoPaginatedAsync(filter);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAtestadoAsync([FromBody] AtestadoAllFilter filter)
    {
      try
      {
        Response<IEnumerable<AtestadoTableDTO>> result = await _atestadoService.GetAllAtestadoAsync(filter);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAtestadoAsync(Guid id)
    {
      Response<AtestadoDTO> result = await _atestadoService.GetAtestadoAsync(id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAtestadoAsync(CreateAtestadoRequest request)
    {
      try
      {
        Response<Guid> result = await _atestadoService.CreateAtestadoAsync(request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAtestadoAsync(Guid id)
    {
      try
      {
        Response<Guid> response = await _atestadoService.DeleteAtestadoAsync(id);
        return Ok(response);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
