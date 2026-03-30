using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Consultas
{
    /// <summary>
    /// API para Tipos de Consulta. Permite ver e editar. Sem modal de inserção.
    /// </summary>
    [Route("client/consultas/[controller]")]
    [ApiController]
    public class TipoConsultaController(ITipoConsultaService tipoConsultaService) : ControllerBase
    {
        private readonly ITipoConsultaService _tipoConsultaService = tipoConsultaService;

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoConsultaAsync([FromBody] TipoConsultaAllFilter? filter = null)
        {
            Response<IEnumerable<TipoConsultaTableDTO>> result = await _tipoConsultaService.GetAllTipoConsultaAsync(filter ?? new TipoConsultaAllFilter());
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoConsultaPaginatedAsync(TipoConsultaTableFilter filter)
        {
            PaginatedResponse<TipoConsultaTableDTO> result = await _tipoConsultaService.GetTipoConsultaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoConsultaAsync(Guid id)
        {
            Response<TipoConsultaDTO> result = await _tipoConsultaService.GetTipoConsultaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoConsultaAsync([FromRoute] Guid id, [FromBody] UpdateTipoConsultaRequest request)
        {
            try
            {
                Response<Guid> result = await _tipoConsultaService.UpdateTipoConsultaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
