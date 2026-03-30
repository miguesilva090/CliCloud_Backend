using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Habilitacoes
{
    [Route("client/habilitacoes/[controller]")]
    [ApiController]
    public class HabilitacaoController(IHabilitacaoService HabilitacaoService) : ControllerBase
    {
        private readonly IHabilitacaoService _HabilitacaoService = HabilitacaoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHabilitacaoAsync(string keyword = "")
        {
            Response<IEnumerable<HabilitacaoDTO>> result = await _HabilitacaoService.GetHabilitacaoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHabilitacaoLightAsync(string keyword = "")
        {
            Response<IEnumerable<HabilitacaoLightDTO>> result = await _HabilitacaoService.GetHabilitacaoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHabilitacaoPaginatedAsync(HabilitacaoTableFilter filter)
        {
            PaginatedResponse<HabilitacaoTableDTO> result = await _HabilitacaoService.GetHabilitacaoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHabilitacaoAsync([FromBody] HabilitacaoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<HabilitacaoTableDTO>> result = await _HabilitacaoService.GetAllHabilitacaoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabilitacaoAsync(Guid id)
        {
            Response<HabilitacaoDTO> result = await _HabilitacaoService.GetHabilitacaoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHabilitacaoAsync(CreateHabilitacaoRequest request)
        {
            try
            {
                Response<Guid> result = await _HabilitacaoService.CreateHabilitacaoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabilitacaoAsync([FromRoute] Guid id, [FromBody] UpdateHabilitacaoRequest request)
        {
            try
            {
                Response<Guid> result = await _HabilitacaoService.UpdateHabilitacaoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabilitacaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HabilitacaoService.DeleteHabilitacaoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleHabilitacaoAsync([FromBody] DeleteMultipleHabilitacaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _HabilitacaoService.DeleteMultipleHabilitacaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
