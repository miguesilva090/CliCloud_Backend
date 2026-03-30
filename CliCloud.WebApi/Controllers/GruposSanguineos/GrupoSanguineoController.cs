using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.GrupoSanguineoService;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.GruposSanguineos
{
    [Route("client/grupossanguineos/[controller]")]
    [ApiController]
    public class GrupoSanguineoController(IGrupoSanguineoService GrupoSanguineoService) : ControllerBase
    {
        private readonly IGrupoSanguineoService _GrupoSanguineoService = GrupoSanguineoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGrupoSanguineoAsync(string keyword = "")
        {
            Response<IEnumerable<GrupoSanguineoDTO>> result = await _GrupoSanguineoService.GetGrupoSanguineoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetGrupoSanguineoLightAsync(string keyword = "")
        {
            Response<IEnumerable<GrupoSanguineoLightDTO>> result = await _GrupoSanguineoService.GetGrupoSanguineoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGrupoSanguineoPaginatedAsync(GrupoSanguineoTableFilter filter)
        {
            PaginatedResponse<GrupoSanguineoTableDTO> result = await _GrupoSanguineoService.GetGrupoSanguineoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllGrupoSanguineoAsync([FromBody] GrupoSanguineoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<GrupoSanguineoTableDTO>> result = await _GrupoSanguineoService.GetAllGrupoSanguineoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrupoSanguineoAsync(Guid id)
        {
            Response<GrupoSanguineoDTO> result = await _GrupoSanguineoService.GetGrupoSanguineoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGrupoSanguineoAsync(CreateGrupoSanguineoRequest request)
        {
            try
            {
                Response<Guid> result = await _GrupoSanguineoService.CreateGrupoSanguineoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrupoSanguineoAsync([FromRoute] Guid id, [FromBody] UpdateGrupoSanguineoRequest request)
        {
            try
            {
                Response<Guid> result = await _GrupoSanguineoService.UpdateGrupoSanguineoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupoSanguineoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _GrupoSanguineoService.DeleteGrupoSanguineoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleGrupoSanguineoAsync([FromBody] DeleteMultipleGrupoSanguineoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _GrupoSanguineoService.DeleteMultipleGrupoSanguineoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
