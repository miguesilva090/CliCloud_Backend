using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.PatologiaService;
using CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs;
using CliCloud.Application.Services.Tratamentos.PatologiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
  [Route("client/tratamentos/[controller]")]
  [ApiController]
  public class PatologiaController(IPatologiaService patologiaService) : ControllerBase
  {
    private readonly IPatologiaService _patologiaService = patologiaService;

    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetPatologiaAsync(string keyword = "")
    {
      var result = await _patologiaService.GetPatologiaAsync(keyword);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetPatologiaLightAsync(string keyword = "")
    {
      var result = await _patologiaService.GetPatologiaLightAsync(keyword);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPatologiaPaginatedAsync(PatologiaTableFilter filter)
    {
      var result = await _patologiaService.GetPatologiaPaginatedAsync(filter);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllPatologiaAsync([FromBody] PatologiaAllFilter filter)
    {
      try
      {
        var result = await _patologiaService.GetAllPatologiaAsync(filter);
        return Ok(result);
      }
      catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatologiaAsync(Guid id)
    {
      var result = await _patologiaService.GetPatologiaAsync(id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreatePatologiaAsync(CreatePatologiaRequest request)
    {
      try
      {
        var result = await _patologiaService.CreatePatologiaAsync(request);
        return Ok(result);
      }
      catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatologiaAsync(UpdatePatologiaRequest request, Guid id)
    {
      try
      {
        var result = await _patologiaService.UpdatePatologiaAsync(request, id);
        return Ok(result);
      }
      catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatologiaAsync(Guid id)
    {
      try
      {
        var result = await _patologiaService.DeletePatologiaAsync(id);
        return Ok(result);
      }
      catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultiplePatologiaAsync([FromBody] DeleteMultiplePatologiaRequest request)
    {
      try
      {
        var result = await _patologiaService.DeleteMultiplePatologiaAsync(request.Ids);
        return Ok(result);
      }
      catch (Exception ex) { return BadRequest(ex.Message); }
    }
  }
}
