using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Faturacao.ZonaComercialService;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.Filters;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/[controller]")]
[ApiController]
public class ZonaComercialController(IZonaComercialService service) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetAsync(string keyword = "")
        => Ok(await service.GetAsync(keyword));

    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetLightAsync(string keyword = "")
        => Ok(await service.GetLightAsync(keyword));

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(ZonaComercialTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync([FromBody] ZonaComercialAllFilter? filter = null)
        => Ok(await service.GetAllAsync(filter ?? new ZonaComercialAllFilter()));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateZonaComercialRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateZonaComercialRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleZonaComercialRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));
}