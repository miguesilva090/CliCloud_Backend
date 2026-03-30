using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Profissoes.ProfissaoService;
using CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using CliCloud.Application.Services.Profissoes.ProfissaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Profissoes
{
    [Route("client/profissoes/[controller]")]
    [ApiController]
    public class ProfissaoController(IProfissaoService ProfissaoService) : ControllerBase
    {
        private readonly IProfissaoService _ProfissaoService = ProfissaoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetProfissaoAsync(string keyword = "")
        {
            Response<IEnumerable<ProfissaoDTO>> result = await _ProfissaoService.GetProfissaoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetProfissaoLightAsync(string keyword = "")
        {
            Response<IEnumerable<ProfissaoLightDTO>> result = await _ProfissaoService.GetProfissaoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetProfissaoPaginatedAsync(ProfissaoTableFilter filter)
        {
            PaginatedResponse<ProfissaoTableDTO> result = await _ProfissaoService.GetProfissaoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllProfissaoAsync([FromBody] ProfissaoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<ProfissaoTableDTO>> result = await _ProfissaoService.GetAllProfissaoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfissaoAsync(Guid id)
        {
            Response<ProfissaoDTO> result = await _ProfissaoService.GetProfissaoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateProfissaoAsync(CreateProfissaoRequest request)
        {
            try
            {
                Response<Guid> result = await _ProfissaoService.CreateProfissaoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfissaoAsync([FromRoute] Guid id, [FromBody] UpdateProfissaoRequest request)
        {
            try
            {
                Response<Guid> result = await _ProfissaoService.UpdateProfissaoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfissaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ProfissaoService.DeleteProfissaoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleProfissaoAsync([FromBody] DeleteMultipleProfissaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _ProfissaoService.DeleteMultipleProfissaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
