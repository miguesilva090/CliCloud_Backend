using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class PeriocidadeTratamentoController(IPeriocidadeTratamentoService PeriocidadeTratamentoService) : ControllerBase
    {
        private readonly IPeriocidadeTratamentoService _PeriocidadeTratamentoService = PeriocidadeTratamentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetPeriocidadeTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<PeriocidadeTratamentoDTO>> result = await _PeriocidadeTratamentoService.GetPeriocidadeTratamentoAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetPeriocidadeTratamentoLightAsync(string keyword = "")
        {
            Response<IEnumerable<PeriocidadeTratamentoLightDTO>> result = await _PeriocidadeTratamentoService.GetPeriocidadeTratamentoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPeriocidadeTratamentoPaginatedAsync(PeriocidadeTratamentoTableFilter filter)
        {
            PaginatedResponse<PeriocidadeTratamentoTableDTO> result = await _PeriocidadeTratamentoService.GetPeriocidadeTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        // all list (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllPeriocidadeTratamentoAsync([FromBody] PeriocidadeTratamentoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<PeriocidadeTratamentoTableDTO>> result = await _PeriocidadeTratamentoService.GetAllPeriocidadeTratamentoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPeriocidadeTratamentoAsync(Guid id)
        {
            Response<PeriocidadeTratamentoDTO> result = await _PeriocidadeTratamentoService.GetPeriocidadeTratamentoAsync(id);
            return Ok(result);
        }

        // single by Descricao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetPeriocidadeTratamentoByDescricaoAsync(string descricao)
        {
            try
            {
                Response<PeriocidadeTratamentoDTO> result = await _PeriocidadeTratamentoService.GetPeriocidadeTratamentoByDescricaoAsync(descricao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreatePeriocidadeTratamentoAsync(CreatePeriocidadeTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _PeriocidadeTratamentoService.CreatePeriocidadeTratamentoAsync(request);
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
        public async Task<IActionResult> UpdatePeriocidadeTratamentoAsync(UpdatePeriocidadeTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _PeriocidadeTratamentoService.UpdatePeriocidadeTratamentoAsync(request, id);
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
        public async Task<IActionResult> DeletePeriocidadeTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _PeriocidadeTratamentoService.DeletePeriocidadeTratamentoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultiplePeriocidadeTratamentoAsync([FromBody] DeleteMultiplePeriocidadeTratamentoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _PeriocidadeTratamentoService.DeleteMultiplePeriocidadeTratamentoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
