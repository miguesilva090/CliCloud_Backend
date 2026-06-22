using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Filters;

namespace CliCloud.WebApi.Controllers.Stocks;

[Route("client/stocks/[controller]")]
[ApiController]
public class UnidadeMedidaController(IUnidadeMedidaService service) : ControllerBase
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
    public async Task<IActionResult> GetPaginatedAsync(UnidadeMedidaTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync([FromBody] UnidadeMedidaAllFilter? filter = null)
        => Ok(await service.GetAllAsync(filter ?? new UnidadeMedidaAllFilter()));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUnidadeMedidaRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateUnidadeMedidaRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleUnidadeMedidaRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));
}
