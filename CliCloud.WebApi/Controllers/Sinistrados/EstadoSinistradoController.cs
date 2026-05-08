using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.Filters;

namespace CliCloud.WebApi.Controllers.Sinistrados
{
    [Route("client/sinistrados/[controller]")]
    [ApiController]
    public class EstadoSinistroController(IEstadoSinistroService service) : ControllerBase
    {
        private readonly IEstadoSinistroService _service = service;

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEstadoSinistroPaginatedAsync(EstadoSinistroTableFilter filter)
            => Ok(await _service.GetPaginatedAsync(filter));
        
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById (Guid id)
            => Ok(await _service.GetByIdAsync(id));
        
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] CreateEstadoSinistroRequest request)
            => Ok(await _service.CreateAsync(request));
        
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEstadoSinistroRequest request)
            => Ok(await _service.UpdateAsync(id, request));

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));

    }
}