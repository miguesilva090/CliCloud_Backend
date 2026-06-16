using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Filters;

namespace CliCloud.WebApi.Controllers.Pagamentos;

[Route("client/pagamentos/[controller]")]
[ApiController]
public class ModoPagamentoController(IModoPagamentoService service) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetAsync(string keyword = "")
        => Ok(await service.GetAsync(keyword));

    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetLightAsync(string keyword = "", bool apenasAtivos = false)
        => Ok(await service.GetLightAsync(keyword, apenasAtivos));

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(ModoPagamentoTableFilter filter)
        => Ok(await service.GetPaginatedAsync(filter));

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAsync([FromBody] ModoPagamentoAllFilter? filter = null)
        => Ok(await service.GetAllAsync(filter ?? new ModoPagamentoAllFilter()));

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
        => Ok(await service.GetAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateModoPagamentoRequest request)
        => Ok(await service.CreateAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateModoPagamentoRequest request, Guid id)
        => Ok(await service.UpdateAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await service.DeleteAsync(id));

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleModoPagamentoRequest request)
        => Ok(await service.DeleteMultipleAsync(request.Ids));

    [Authorize(Roles = "client")]
    [HttpPost("{id}/historico/passar")]
    public async Task<IActionResult> PassarHistoricoAsync(Guid id)
        => Ok(await service.PassarHistoricoAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost("{id}/historico/retirar")]
    public async Task<IActionResult> RetirarHistoricoAsync(Guid id)
        => Ok(await service.RetirarHistoricoAsync(id));
}
