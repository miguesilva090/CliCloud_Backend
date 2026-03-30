using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.GoniometriasService;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class GoniometriasController(IGoniometriasService GoniometriasService) : ControllerBase
    {
        private readonly IGoniometriasService _GoniometriasService = GoniometriasService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGoniometriasAsync(string keyword = "")
        {
            Response<IEnumerable<GoniometriasDTO>> result = await _GoniometriasService.GetGoniometriasAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetGoniometriasLightAsync(string keyword = "") 
        {
            Response<IEnumerable<GoniometriasLightDTO>> result = await _GoniometriasService.GetGoniometriasLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGoniometriasPaginatedAsync(GoniometriasTableFilter filter)
        {
            PaginatedResponse<GoniometriasTableDTO> result = await _GoniometriasService.GetGoniometriasPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllGoniometriasAsync([FromBody] GoniometriasAllFilter filter)
        {
            try
            {
                var result = await _GoniometriasService.GetAllGoniometriasAsync(filter);
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
        public async Task<IActionResult> GetGoniometriasAsync(Guid id)
        {
            Response<GoniometriasDTO> result = await _GoniometriasService.GetGoniometriasAsync(id);
            return Ok(result);
        }

        // single by Descricao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetGoniometriasByDescricaoAsync(string descricao)
        {
            Response<GoniometriasDTO> result = await _GoniometriasService.GetGoniometriasByDescricaoAsync(descricao);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGoniometriasAsync(CreateGoniometriasRequest request)
        {
            try
            {
                Response<Guid> result = await _GoniometriasService.CreateGoniometriasAsync(request);
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
        public async Task<IActionResult> UpdateGoniometriasAsync(UpdateGoniometriasRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _GoniometriasService.UpdateGoniometriasAsync(request, id);
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
        public async Task<IActionResult> DeleteGoniometriasAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _GoniometriasService.DeleteGoniometriasAsync(id);
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
        public async Task<IActionResult> DeleteMultipleGoniometriasAsync([FromBody] DeleteMultipleGoniometriasRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _GoniometriasService.DeleteMultipleGoniometriasAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
