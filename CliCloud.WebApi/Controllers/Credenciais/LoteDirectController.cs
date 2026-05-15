using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Credenciais
{
    [Route("client/credenciais/[controller]")]
    [ApiController]
    public class LoteDirectController(ILoteDirectService service) : ControllerBase
    {
        private readonly ILoteDirectService _service = service;

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPaginated([FromBody] LoteDirectTableFilter filter)
            => Ok(await _service.GetPaginatedAsync(filter));
        
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoteDirectRequest request)
            => Ok(await _service.CreateAsync(request));

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLoteDirectRequest request)
            => Ok(await _service.UpdateAsync(id, request));

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));

        [Authorize(Roles = "client")]
        [HttpPost("corrigir-lotes")]
        public async Task<IActionResult> CorrigirLotes([FromBody] CorrigirLotesRequest request)
            => Ok(await _service.CorrigirLotesAsync(request));

        [Authorize(Roles = "client")]
        [HttpGet("tipos-lote/light")]
        public async Task<IActionResult> GetTiposLoteLight()
            => Ok(await _service.GetTiposLoteLightAsync());
    }
}