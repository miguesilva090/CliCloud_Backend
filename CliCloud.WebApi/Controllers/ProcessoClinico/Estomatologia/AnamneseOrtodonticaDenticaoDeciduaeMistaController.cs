using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOrtodonticaDenticaoDeciduaeMistaController(IAnamneseOrtodonticaDenticaoDeciduaeMistaService anamneseOrtodonticaDenticaoDeciduaeMistaService) : ControllerBase
    {
        private readonly IAnamneseOrtodonticaDenticaoDeciduaeMistaService _anamneseOrtodonticaDenticaoDeciduaeMistaService = anamneseOrtodonticaDenticaoDeciduaeMistaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>> result = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOrtodonticaDenticaoDeciduaeMistaPaginatedAsync(AnamneseOrtodonticaDenticaoDeciduaeMistaTableFilter filter)
        {
            PaginatedResponse<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO> result = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.GetAnamneseOrtodonticaDenticaoDeciduaeMistaPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id)
        {
            Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO> result = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(id);
            return Ok(result);
        }

        // single by utente 
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaDenticaoDeciduaeMistaByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO?> result = 
                await _anamneseOrtodonticaDenticaoDeciduaeMistaService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync([FromBody] CreateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.CreateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync([FromBody] UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOrtodonticaDenticaoDeciduaeMistaService.DeleteAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
