using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.GrauAlergiaService;
using CliCloud.Application.Services.GrauAlergiaService.DTOs;
using CliCloud.Application.Services.GrauAlergiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Alergias
{
    [Route("client/alergias/[controller]")]
    [ApiController]
    public class GrauAlergiaController(IGrauAlergiaService grauAlergiaService) : ControllerBase
    {
        private readonly IGrauAlergiaService _grauAlergiaService = grauAlergiaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGrauAlergiaAsync(string keyword = "")
        {
            Response<IEnumerable<GrauAlergiaDTO>> result = await _grauAlergiaService.GetGrauAlergiaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetGrauAlergiaLightAsync(string keyword = "")
        {
            Response<IEnumerable<GrauAlergiaLightDTO>> result = await _grauAlergiaService.GetGrauAlergiaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGrauAlergiaPaginatedAsync(GrauAlergiaTableFilter filter)
        {
            PaginatedResponse<GrauAlergiaTableDTO> result = await _grauAlergiaService.GetGrauAlergiaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllGrauAlergiaAsync([FromBody] GrauAlergiaAllFilter filter)
        {
            try
            {
                Response<IEnumerable<GrauAlergiaTableDTO>> result = await _grauAlergiaService.GetAllGrauAlergiaAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrauAlergiaAsync(Guid id)
        {
            Response<GrauAlergiaDTO> result = await _grauAlergiaService.GetGrauAlergiaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGrauAlergiaAsync(CreateGrauAlergiaRequest request)
        {
            try
            {
                Response<Guid> result = await _grauAlergiaService.CreateGrauAlergiaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrauAlergiaAsync(Guid id, [FromBody] UpdateGrauAlergiaRequest request)
        {
            try
            {
                Response<Guid> result = await _grauAlergiaService.UpdateGrauAlergiaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrauAlergiaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _grauAlergiaService.DeleteGrauAlergiaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleGrauAlergiaAsync([FromBody] DeleteMultipleGrauAlergiaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _grauAlergiaService.DeleteMultipleGrauAlergiaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
