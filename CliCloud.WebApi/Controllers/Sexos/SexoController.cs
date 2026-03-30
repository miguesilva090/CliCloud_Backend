using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Sexos.SexoService;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Sexos
{
    [Route("client/sexos/[controller]")]
    [ApiController]
    public class SexoController(ISexoService sexoService) : ControllerBase
    {
        private readonly ISexoService _sexoService = sexoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetSexoAsync(string keyword = "")
        {
            Response<IEnumerable<SexoDTO>> result = await _sexoService.GetSexoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetSexoLightAsync(string keyword = "")
        {
            Response<IEnumerable<SexoLightDTO>> result = await _sexoService.GetSexoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetSexoPaginatedAsync(SexoTableFilter filter)
        {
            PaginatedResponse<SexoTableDTO> result = await _sexoService.GetSexoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllSexoAsync([FromBody] SexoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<SexoTableDTO>> result = await _sexoService.GetAllSexoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSexoAsync(Guid id)
        {
            Response<SexoDTO> result = await _sexoService.GetSexoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateSexoAsync(CreateSexoRequest request)
        {
            try
            {
                Response<Guid> result = await _sexoService.CreateSexoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSexoAsync([FromRoute] Guid id, [FromBody] UpdateSexoRequest request)
        {
            try
            {
                Response<Guid> result = await _sexoService.UpdateSexoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSexoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _sexoService.DeleteSexoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleSexoAsync([FromBody] DeleteMultipleSexoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _sexoService.DeleteMultipleSexoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

