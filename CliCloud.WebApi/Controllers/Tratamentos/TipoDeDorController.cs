using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class TipoDeDorController(ITipoDeDorService TipoDeDorService) : ControllerBase
    {
        private readonly ITipoDeDorService _TipoDeDorService = TipoDeDorService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoDeDorAsync(string keyword = "")
        {
            Response<IEnumerable<TipoDeDorDTO>> result = await _TipoDeDorService.GetTipoDeDorAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoDeDorLightAsync(string keyword = "")
        {
            Response<IEnumerable<TipoDeDorLightDTO>> result = await _TipoDeDorService.GetTipoDeDorLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoDeDorPaginatedAsync(TipoDeDorTableFilter filter)
        {
            PaginatedResponse<TipoDeDorTableDTO> result = await _TipoDeDorService.GetTipoDeDorPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoDeDorAsync([FromBody] TipoDeDorAllFilter filter)
        {
            try
            {
                var result = await _TipoDeDorService.GetAllTipoDeDorAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTipoDeDorByIdAsync(Guid id)
        {
            Response<TipoDeDorDTO> result = await _TipoDeDorService.GetTipoDeDorAsync(id);
            return Ok(result);
        }

        // single by Descricao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetTipoDeDorByDescricaoAsync(string descricao)
        {
            Response<TipoDeDorDTO> result = await _TipoDeDorService.GetTipoDeDorByDescricaoAsync(descricao);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoDeDorAsync(CreateTipoDeDorRequest request)
        {
            try
            {
                Response<Guid> result = await _TipoDeDorService.CreateTipoDeDorAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTipoDeDorAsync(UpdateTipoDeDorRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TipoDeDorService.UpdateTipoDeDorAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTipoDeDorAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TipoDeDorService.DeleteTipoDeDorAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTipoDeDorAsync([FromBody] DeleteMultipleTipoDeDorRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _TipoDeDorService.DeleteMultipleTipoDeDorAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
