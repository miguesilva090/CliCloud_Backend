using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Sinistros.SinistradoService;
using CliCloud.Application.Services.Sinistros.SinistradoService.DTOs;
using CliCloud.Application.Services.Sinistros.SinistradoService.Filters;


namespace CliCloud.WebApi.Controllers.Sinistrados
{
    [Route("client/sinistrados/[controller]")]
    [ApiController]
    public class SinistradoController(ISinistradoService service) : ControllerBase
    {
        private readonly ISinistradoService _service = service;

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPaginated([FromBody] SinistradoTableFilter filter)
            => Ok(await _service.GetPaginatedAsync(filter));
        
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Roles = "client")]
        [HttpGet("next-codigo")]
        public async Task<IActionResult> GetNextCodigo()
            => Ok(await _service.GetNextCodigoSinistroAsync());
        
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSinistradoRequest request)
            => Ok(await _service.CreateAsync(request));

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSinistradoRequest request)
            => Ok(await _service.UpdateAsync(id, request));
        
        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/move-to-history")]
        public async Task<IActionResult> MoveToHistory(Guid id)
            => Ok(await _service.MoveToHistoryAsync(id));

        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/restore-from-history")]
        public async Task<IActionResult> RestoreFromHistory(Guid id)
            => Ok(await _service.RestoreFromHistoryAsync(id));
        
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) 
            => Ok(await _service.DeleteAsync(id));

        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}/servicos-nao-faturados")]
        public async Task<IActionResult> GetUnbilledServicesByUtenteId(Guid utenteId)
            => Ok(await _service.GetUnbilledServicesByUtenteIdAsync(utenteId));
    }
}