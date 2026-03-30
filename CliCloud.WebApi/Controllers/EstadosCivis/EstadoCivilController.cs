using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.EstadosCivis
{
    [Route("client/estadoscivis/[controller]")]
    [ApiController]
    public class EstadoCivilController(IEstadoCivilService EstadoCivilService) : ControllerBase
    {
        private readonly IEstadoCivilService _EstadoCivilService = EstadoCivilService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEstadoCivilAsync(string keyword = "")
        {
            Response<IEnumerable<EstadoCivilDTO>> result = await _EstadoCivilService.GetEstadoCivilAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEstadoCivilLightAsync(string keyword = "")
        {
            Response<IEnumerable<EstadoCivilLightDTO>> result = await _EstadoCivilService.GetEstadoCivilLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEstadoCivilPaginatedAsync(EstadoCivilTableFilter filter)
        {
            PaginatedResponse<EstadoCivilTableDTO> result = await _EstadoCivilService.GetEstadoCivilPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEstadoCivilAsync([FromBody] EstadoCivilAllFilter filter)
        {
            try
            {
                Response<IEnumerable<EstadoCivilTableDTO>> result = await _EstadoCivilService.GetAllEstadoCivilAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstadoCivilAsync(Guid id)
        {
            Response<EstadoCivilDTO> result = await _EstadoCivilService.GetEstadoCivilAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEstadoCivilAsync(CreateEstadoCivilRequest request)
        {
            try
            {
                Response<Guid> result = await _EstadoCivilService.CreateEstadoCivilAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstadoCivilAsync([FromRoute] Guid id, [FromBody] UpdateEstadoCivilRequest request)
        {
            try
            {
                Response<Guid> result = await _EstadoCivilService.UpdateEstadoCivilAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoCivilAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EstadoCivilService.DeleteEstadoCivilAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleEstadoCivilAsync([FromBody] DeleteMultipleEstadoCivilRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _EstadoCivilService.DeleteMultipleEstadoCivilAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
