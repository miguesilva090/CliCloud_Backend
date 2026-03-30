using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Moedas.MoedaService;
using CliCloud.Application.Services.Moedas.MoedaService.DTOs;
using CliCloud.Application.Services.Moedas.MoedaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Moedas
{
    [Route("client/moedas/[controller]")]
    [ApiController]
    public class MoedaController(IMoedaService MoedaService) : ControllerBase
    {
        private readonly IMoedaService _MoedaService = MoedaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMoedaAsync(string keyword = "")
        {
            Response<IEnumerable<MoedaDTO>> result = await _MoedaService.GetMoedaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMoedaLightAsync(string keyword = "")
        {
            Response<IEnumerable<MoedaLightDTO>> result = await _MoedaService.GetMoedaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMoedaPaginatedAsync(MoedaTableFilter filter)
        {
            PaginatedResponse<MoedaTableDTO> result = await _MoedaService.GetMoedaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMoedaAsync([FromBody] MoedaAllFilter filter)
        {
            try
            {
                Response<IEnumerable<MoedaTableDTO>> result = await _MoedaService.GetAllMoedaAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMoedaAsync(Guid id)
        {
            Response<MoedaDTO> result = await _MoedaService.GetMoedaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMoedaAsync(CreateMoedaRequest request)
        {
            try
            {
                Response<Guid> result = await _MoedaService.CreateMoedaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMoedaAsync([FromRoute] Guid id, [FromBody] UpdateMoedaRequest request)
        {
            try
            {
                Response<Guid> result = await _MoedaService.UpdateMoedaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoedaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MoedaService.DeleteMoedaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleMoedaAsync([FromBody] DeleteMultipleMoedaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _MoedaService.DeleteMultipleMoedaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
