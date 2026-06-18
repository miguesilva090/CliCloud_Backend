using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Filters;

namespace CliCloud.WebApi.Controllers.Stocks;

[Route("client/stocks/[controller]")]
[ApiController]
public class FamiliaArtigoController(IFamiliaArtigoService service) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetLightAsync(string keyword = "")
        => Ok(await service.GetLightAsync(keyword));

    [Authorize(Roles = "client")]
    [HttpGet("ancestors")]
    public async Task<IActionResult> GetAncestorsAsync([FromQuery] Guid? parentId = null)
        => Ok(await service.GetAncestorsAsync(parentId));

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(FamiliaArtigoTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync([FromBody] FamiliaArtigoAllFilter? filter = null)
        => Ok(await service.GetAllAsync(filter ?? new FamiliaArtigoAllFilter()));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateFamiliaArtigoRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateFamiliaArtigoRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleFamiliaArtigoRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));
}