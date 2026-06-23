using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Filters;

namespace CliCloud.WebApi.Controllers.Stocks;

[Route("client/stocks/[controller]")]
[ApiController]
public class SubsistemaArtigoController(ISubsistemaArtigoService service ) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(SubsistemaArtigoTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSubsistemaArtigoRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateSubsistemaArtigoRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleSubsistemaArtigoRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));
}