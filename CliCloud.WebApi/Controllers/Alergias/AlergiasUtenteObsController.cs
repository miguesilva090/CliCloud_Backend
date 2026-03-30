using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.AlergiasUtenteObsService;
using CliCloud.Application.Services.AlergiasUtenteObsService.DTOs;
using CliCloud.Application.Services.AlergiasUtenteObsService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Alergias
{
    [Route("client/alergias/[controller]")]
    [ApiController]
    public class AlergiasUtenteObsController(IAlergiasUtenteObsService alergiasUtenteObsService) : ControllerBase
    {
        private readonly IAlergiasUtenteObsService _alergiasUtenteObsService = alergiasUtenteObsService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAlergiasUtenteObsAsync(string keyword = "")
        {
            Response<IEnumerable<AlergiasUtenteObsDTO>> result = await _alergiasUtenteObsService.GetAlergiasUtenteObsAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAlergiasUtenteObsPaginatedAsync(AlergiasUtenteObsTableFilter filter)
        {
            PaginatedResponse<AlergiasUtenteObsDTO> result = await _alergiasUtenteObsService.GetAlergiasUtenteObsPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlergiasUtenteObsAsync(Guid id)
        {
            Response<AlergiasUtenteObsDTO> result = await _alergiasUtenteObsService.GetAlergiasUtenteObsAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("by-utente/{utenteId}")]
        public async Task<IActionResult> GetByUtenteIdAsync(Guid utenteId)
        {
            Response<AlergiasUtenteObsDTO?> result = await _alergiasUtenteObsService.GetByUtenteIdAsync(utenteId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAlergiasUtenteObsAsync(CreateAlergiasUtenteObsRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiasUtenteObsService.CreateAlergiasUtenteObsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlergiasUtenteObsAsync(Guid id, [FromBody] UpdateAlergiasUtenteObsRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiasUtenteObsService.UpdateAlergiasUtenteObsAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlergiasUtenteObsAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _alergiasUtenteObsService.DeleteAlergiasUtenteObsAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
