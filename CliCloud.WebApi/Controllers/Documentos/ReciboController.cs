using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Documentos.ReciboService;
using CliCloud.Application.Services.Documentos.ReciboService.DTOs;
using CliCloud.Application.Services.Documentos.ReciboService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Documentos
{
    [Route("client/documentos/[controller]")]
    [ApiController]
    public class ReciboController(IReciboService ReciboService) : ControllerBase
    {
        private readonly IReciboService _ReciboService = ReciboService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetReciboAsync(string keyword = "")
        {
            Response<IEnumerable<ReciboDTO>> result = await _ReciboService.GetReciboAsync(keyword);
            return Ok(result);
        }
        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetReciboLightAsync(string keyword = "")
        {
          Response<IEnumerable<ReciboLightDTO>> result = await _ReciboService.GetReciboLightAsync(keyword);
          return Ok(result);
        }
        // All Recibos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllReciboAsync([FromBody] ReciboAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<ReciboTableDTO>> result = await _ReciboService.GetAllReciboAsync(filter ?? new ReciboAllFilter());
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetReciboPaginatedAsync(ReciboTableFilter filter)
        {
            PaginatedResponse<ReciboTableDTO> result = await _ReciboService.GetReciboPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReciboAsync(Guid id)
        {
            Response<ReciboDTO> result = await _ReciboService.GetReciboAsync(id);
            return Ok(result);
        }
    }
}
