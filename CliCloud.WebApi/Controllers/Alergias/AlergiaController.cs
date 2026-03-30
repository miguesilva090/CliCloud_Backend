using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Alergias.AlergiaService;
using CliCloud.Application.Services.Alergias.AlergiaService.DTOs;
using CliCloud.Application.Services.Alergias.AlergiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Alergias
{
    [Route("client/alergias/[controller]")]
    [ApiController]
    public class AlergiaController(IAlergiaService alergiaService) : ControllerBase
    {
        private readonly IAlergiaService _alergiaService = alergiaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAlergiaAsync(string keyword = "")
        {
            Response<IEnumerable<AlergiaDTO>> result = await _alergiaService.GetAlergiaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAlergiaLightAsync(string keyword = "")
        {
            Response<IEnumerable<AlergiaLightDTO>> result = await _alergiaService.GetAlergiaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAlergiaPaginatedAsync(AlergiaTableFilter filter)
        {
            PaginatedResponse<AlergiaTableDTO> result = await _alergiaService.GetAlergiaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlergiaAsync(Guid id)
        {
            Response<AlergiaDTO> result = await _alergiaService.GetAlergiaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAlergiaAsync(CreateAlergiaRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiaService.CreateAlergiaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlergiaAsync(Guid id, UpdateAlergiaRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiaService.UpdateAlergiaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlergiaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _alergiaService.DeleteAlergiaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAlergiaAsync([FromBody] DeleteMultipleAlergiaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _alergiaService.DeleteMultipleAlergiaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
