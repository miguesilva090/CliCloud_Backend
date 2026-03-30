using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.GrausParentesco
{
    [Route("client/graus-parentesco/[controller]")]
    [ApiController]
    public class GrauParentescoController(IGrauParentescoService grauParentescoService) : ControllerBase
    {
        private readonly IGrauParentescoService _grauParentescoService = grauParentescoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGrauParentescoAsync(string keyword = "")
        {
            Response<IEnumerable<GrauParentescoDTO>> result = await _grauParentescoService.GetGrauParentescoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetGrauParentescoLightAsync(string keyword = "")
        {
            Response<IEnumerable<GrauParentescoLightDTO>> result = await _grauParentescoService.GetGrauParentescoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGrauParentescoPaginatedAsync(GrauParentescoTableFilter filter)
        {
            PaginatedResponse<GrauParentescoTableDTO> result = await _grauParentescoService.GetGrauParentescoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllGrauParentescoAsync([FromBody] GrauParentescoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<GrauParentescoTableDTO>> result = await _grauParentescoService.GetAllGrauParentescoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrauParentescoAsync(Guid id)
        {
            Response<GrauParentescoDTO> result = await _grauParentescoService.GetGrauParentescoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGrauParentescoAsync(CreateGrauParentescoRequest request)
        {
            try
            {
                Response<Guid> result = await _grauParentescoService.CreateGrauParentescoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrauParentescoAsync([FromRoute] Guid id, [FromBody] UpdateGrauParentescoRequest request)
        {
            try
            {
                Response<Guid> result = await _grauParentescoService.UpdateGrauParentescoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrauParentescoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _grauParentescoService.DeleteGrauParentescoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleGrauParentescoAsync([FromBody] DeleteMultipleGrauParentescoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _grauParentescoService.DeleteMultipleGrauParentescoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
