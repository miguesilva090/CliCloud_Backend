using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico;

[Route("client/processo-clinico/[controller]")]
[ApiController]
public class SeparadorVinculoController(ISeparadorVinculoService service) : ControllerBase
{
    private readonly ISeparadorVinculoService _service = service;

    [Authorize(Roles = "client")]
    [HttpGet("separador/{separadorId:guid}")]
    public async Task<IActionResult> GetBySeparadorAsync(Guid separadorId)
    {
        Response<IEnumerable<SeparadorVinculoDTO>> result = await _service.GetBySeparadorAsync(separadorId);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSeparadorVinculoRequest request)
    {
        Response<Guid> result = await _service.CreateAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Response<Guid> result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}
