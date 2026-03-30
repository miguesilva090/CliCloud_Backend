using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Especialidades.EspecialidadeService;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Especialidades
{
    [Route("client/especialidades/[controller]")]
    [ApiController]
    public class EspecialidadeController(IEspecialidadeService EspecialidadeService) : ControllerBase
    {
        private readonly IEspecialidadeService _EspecialidadeService = EspecialidadeService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEspecialidadeAsync(string keyword = "")
        {
            Response<IEnumerable<EspecialidadeDTO>> result = await _EspecialidadeService.GetEspecialidadeAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEspecialidadeLightAsync(string keyword = "")
        {
          Response<IEnumerable<EspecialidadeLightDTO>> result = await _EspecialidadeService.GetEspecialidadeLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEspecialidadePaginatedAsync(EspecialidadeTableFilter filter)
        {
            PaginatedResponse<EspecialidadeTableDTO> result = await _EspecialidadeService.GetEspecialidadePaginatedAsync(filter);
            return Ok(result);
        }

        // All Especialidade (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEspecialidadeAsync([FromBody] EspecialidadeAllFilter filter)
        {
          try
          {
            Response<IEnumerable<EspecialidadeTableDTO>> result = await _EspecialidadeService.GetAllEspecialidadeAsync(filter);
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
        public async Task<IActionResult> GetEspecialidadeAsync(Guid id)
        {
            Response<EspecialidadeDTO> result = await _EspecialidadeService.GetEspecialidadeAsync(id);
            return Ok(result);
        }

        //Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetEspecialidadeByNameAsync(string nome)
        {
          Response<IEnumerable<EspecialidadeDTO>> result = await _EspecialidadeService.GetEspecialidadeByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEspecialidadeAsync(CreateEspecialidadeRequest request)
        {
            try
            {
                Response<Guid> result = await _EspecialidadeService.CreateEspecialidadeAsync(request);
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
        public async Task<IActionResult> UpdateEspecialidadeAsync([FromRoute] Guid id, [FromBody] UpdateEspecialidadeRequest request)
        {
            try
            {
                Response<Guid> result = await _EspecialidadeService.UpdateEspecialidadeAsync(request, id);
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
        public async Task<IActionResult> DeleteEspecialidadeAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EspecialidadeService.DeleteEspecialidadeAsync(id);
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
        public async Task<IActionResult> DeleteMultipleEspecialidadeAsync([FromBody] DeleteMultipleEspecialidadeRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _EspecialidadeService.DeleteMultipleEspecialidadeAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
