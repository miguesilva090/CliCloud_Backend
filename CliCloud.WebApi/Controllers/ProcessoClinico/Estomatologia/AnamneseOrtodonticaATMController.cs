using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOrtodonticaATMController(IAnamneseOrtodonticaATMService anamneseOrtodonticaATMService) : ControllerBase
    {
        private readonly IAnamneseOrtodonticaATMService _anamneseOrtodonticaATMService = anamneseOrtodonticaATMService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOrtodonticaATMAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOrtodonticaATMDTO>> result = await _anamneseOrtodonticaATMService.GetAnamneseOrtodonticaATMAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOrtodonticaATMPaginatedAsync(AnamneseOrtodonticaATMTableFilter filter)
        {
            PaginatedResponse<AnamneseOrtodonticaATMDTO> result = await _anamneseOrtodonticaATMService.GetAnamneseOrtodonticaATMPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaATMAsync(Guid id)
        {
            Response<AnamneseOrtodonticaATMDTO> result = await _anamneseOrtodonticaATMService.GetAnamneseOrtodonticaATMAsync(id);
            return Ok(result);
        }

        // single by utente 
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaATMByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOrtodonticaATMDTO?> result = 
                await _anamneseOrtodonticaATMService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOrtodonticaATMAsync([FromBody] CreateAnamneseOrtodonticaATMRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaATMService.CreateAnamneseOrtodonticaATMAsync(request);
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
        public async Task<IActionResult> UpdateAnamneseOrtodonticaATMAsync([FromBody] UpdateAnamneseOrtodonticaATMRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaATMService.UpdateAnamneseOrtodonticaATMAsync(request, id);
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
        public async Task<IActionResult> DeleteAnamneseOrtodonticaATMAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOrtodonticaATMService.DeleteAnamneseOrtodonticaATMAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
