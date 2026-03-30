using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Especialidades
{
    [Route("client/especialidades/[controller]")]
    [ApiController]
    public class CategoriaEspecialidadeController(ICategoriaEspecialidadeService CategoriaEspecialidadeService) : ControllerBase
    {
        private readonly ICategoriaEspecialidadeService _CategoriaEspecialidadeService = CategoriaEspecialidadeService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriaEspecialidadeAsync(string keyword = "")
        {
          Response<IEnumerable<CategoriaEspecialidadeDTO>> result = await _CategoriaEspecialidadeService.GetCategoriaEspecialidadeAsync(keyword);
          return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCategoriaEspecialidadeLightAsync(string keyword = "")
        {
          Response<IEnumerable<CategoriaEspecialidadeLightDTO>> result = await _CategoriaEspecialidadeService.GetCategoriaEspecialidadeLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCategoriaEspecialidadePaginatedAsync(CategoriaEspecialidadeTableFilter filter)
        {
            PaginatedResponse<CategoriaEspecialidadeTableDTO> result = await _CategoriaEspecialidadeService.GetCategoriaEspecialidadePaginatedAsync(filter);
            return Ok(result);
        }

        // All CategoriaEspecialidade (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCategoriaEspecialidadeAsync([FromBody] CategoriaEspecialidadeAllFilter filter)
        {
          try
          {
            Response<IEnumerable<CategoriaEspecialidadeTableDTO>> result = await _CategoriaEspecialidadeService.GetAllCategoriaEspecialidadeAsync(filter);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaEspecialidadeAsync(Guid id)
        {
            Response<CategoriaEspecialidadeDTO> result = await _CategoriaEspecialidadeService.GetCategoriaEspecialidadeAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoriaEspecialidadeAsync(CreateCategoriaEspecialidadeRequest request)
        {
            try
            {
                Response<Guid> result = await _CategoriaEspecialidadeService.CreateCategoriaEspecialidadeAsync(request);
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
        public async Task<IActionResult> UpdateCategoriaEspecialidadeAsync([FromRoute] Guid id, [FromBody] UpdateCategoriaEspecialidadeRequest request)
        {
            try
            {
                Response<Guid> result = await _CategoriaEspecialidadeService.UpdateCategoriaEspecialidadeAsync(request, id);
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
        public async Task<IActionResult> DeleteCategoriaEspecialidadeAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _CategoriaEspecialidadeService.DeleteCategoriaEspecialidadeAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleCategoriaEspecialidadeAsync([FromBody] DeleteMultipleCategoriaEspecialidadeRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _CategoriaEspecialidadeService.DeleteMultipleCategoriaEspecialidadeAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
