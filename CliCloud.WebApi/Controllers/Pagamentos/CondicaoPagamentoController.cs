using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Filters;

namespace CliCloud.WebApi.Controllers.Pagamentos;

[Route("client/pagamentos/[controller]")]
[ApiController]
public class CondicaoPagamentoController(ICondicaoPagamentoService service) : ControllerBase
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
    public async Task<IActionResult> GetPaginatedAsync(CondicaoPagamentoTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync([FromBody] CondicaoPagamentoAllFilter? filter = null)
        => Ok(await service.GetAllAsync(filter ?? new CondicaoPagamentoAllFilter()));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCondicaoPagamentoRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateCondicaoPagamentoRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleCondicaoPagamentoRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));
}
