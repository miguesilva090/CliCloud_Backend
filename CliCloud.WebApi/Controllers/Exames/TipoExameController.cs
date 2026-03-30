using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Exames.TipoExameService;
using CliCloud.Application.Services.Exames.TipoExameService.DTOs;
using CliCloud.Application.Services.Exames.TipoExameService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Exames
{
    [Route("client/exames/[controller]")]
    [ApiController]
    public class TipoExameController(ITipoExameService tipoExameService) : ControllerBase
    {
        private readonly ITipoExameService _tipoExameService = tipoExameService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoExameAsync(string keyword = "")
        {
            Response<IEnumerable<TipoExameDTO>> result = await _tipoExameService.GetTipoExameAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoExameLightAsync(string keyword = "")
        {
            Response<IEnumerable<TipoExameLightDTO>> result = await _tipoExameService.GetTipoExameLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoExamePaginatedAsync(TipoExameTableFilter filter)
        {
            PaginatedResponse<TipoExameTableDTO> result = await _tipoExameService.GetTipoExamePaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoExameAsync([FromBody] TipoExameAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<TipoExameTableDTO>> result = await _tipoExameService.GetAllTipoExameAsync(filter ?? new TipoExameAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoExameAsync(Guid id)
        {
            Response<TipoExameDTO> result = await _tipoExameService.GetTipoExameAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoExameAsync(CreateTipoExameRequest request)
        {
            try
            {
                Response<Guid> result = await _tipoExameService.CreateTipoExameAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoExameAsync(UpdateTipoExameRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _tipoExameService.UpdateTipoExameAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoExameAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _tipoExameService.DeleteTipoExameAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleTipoExameAsync([FromBody] DeleteMultipleTipoExameRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _tipoExameService.DeleteMultipleTipoExameAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
