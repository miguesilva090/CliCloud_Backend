using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class RelatorioAtestadoController(IRelatorioAtestadoService relatorioAtestadoService) : ControllerBase
    {
        private readonly IRelatorioAtestadoService _relatorioAtestadoService = relatorioAtestadoService;

        // full list

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetRelatorioAtestadoAsync(string keyword = "")
        {
            Response<IEnumerable<RelatorioAtestadoDTO>> result = await _relatorioAtestadoService.GetRelatorioAtestadoAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("RelatorioAtestado-paginated")]
        public async Task<IActionResult> GetRelatorioAtestadoPaginatedAsync(RelatorioAtestadoTableFilter filter)
        {
            PaginatedResponse<RelatorioAtestadoDTO> result = await _relatorioAtestadoService.GetRelatorioAtestadoPaginatedAsync(filter);
            return Ok(result);
        }

        // all by utente
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetRelatorioAtestadoByUtenteAsync(Guid utenteId)
        {
            Response<IEnumerable<RelatorioAtestadoDTO>> result = await _relatorioAtestadoService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRelatorioAtestadoAsync(Guid id)
        {
            Response<RelatorioAtestadoDTO> result = await _relatorioAtestadoService.GetRelatorioAtestadoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateRelatorioAtestadoAsync(CreateRelatorioAtestadoRequest request)
        {
            try
            {
                Response<Guid> result = await _relatorioAtestadoService.CreateRelatorioAtestadoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRelatorioAtestadoAsync([FromBody] UpdateRelatorioAtestadoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _relatorioAtestadoService.UpdateRelatorioAtestadoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelatorioAtestadoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _relatorioAtestadoService.DeleteRelatorioAtestadoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // assinar Relatorio/Atestado
        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/assinar")]
        public async Task<IActionResult> AssinarRelatorioAtestadoAsync(Guid id)
        {
            var result = await _relatorioAtestadoService.AssinarRelatorioAtestadoAsync(id);
            return Ok(result);
        }
    }
}
