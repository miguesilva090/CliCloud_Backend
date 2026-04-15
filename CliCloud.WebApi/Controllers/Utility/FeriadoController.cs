using CliCloud.Application.Services.Utility.FeriadoService;
using CliCloud.Application.Services.Utility.FeriadoService.DTOs;
using CliCloud.Application.Services.Utility.FeriadoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class FeriadoController(IFeriadoService service) : ControllerBase
    {
        private readonly IFeriadoService _service = service;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTodosAsync(string keyword = "")
        {
            return Ok(await _service.GetTodosAsync(keyword));
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPaginadoAsync([FromBody] FeriadoTableFilter filter)
        {
            return Ok(await _service.GetPaginadoAsync(filter));
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPorIdAsync(Guid id)
        {
            return Ok(await _service.GetPorIdAsync(id));
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CriarAsync([FromBody] CreateFeriadoRequest request)
        {
            return Ok(await _service.CriarAsync(request));
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAsync(
            [FromBody] UpdateFeriadoRequest request,
            Guid id
        )
        {
            return Ok(await _service.AtualizarAsync(request, id));
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> ApagarAsync(Guid id)
        {
            return Ok(await _service.ApagarAsync(id));
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> ApagarEmLoteAsync([FromBody] IEnumerable<Guid> ids)
        {
            return Ok(await _service.ApagarEmLoteAsync(ids));
        }

        [Authorize(Roles = "client")]
        [HttpPost("inserir-ano")]
        public async Task<IActionResult> InserirAnoAsync(
            [FromBody] InsertFeriadosAnoRequest request
        )
        {
            return Ok(await _service.InserirAnoAsync(request));
        }

        [Authorize(Roles = "client")]
        [HttpPost("importar")]
        public async Task<IActionResult> ImportarAsync([FromBody] ImportFeriadosRequest request)
        {
            return Ok(await _service.ImportarAsync(request));
        }

        [Authorize(Roles = "client")]
        [HttpGet("verificar")]
        public async Task<IActionResult> VerificarSeEFeriadoAsync([FromQuery] DateTime data)
        {
            return Ok(await _service.VerificarSeEFeriadoAsync(data));
        }
    }
}
