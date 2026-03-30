using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOrtodonticaAnaliseGeralController(IAnamneseOrtodonticaAnaliseGeralService anamneseOrtodonticaAnaliseGeralService) : ControllerBase
    {
        private readonly IAnamneseOrtodonticaAnaliseGeralService _anamneseOrtodonticaAnaliseGeralService = anamneseOrtodonticaAnaliseGeralService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseGeralAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOrtodonticaAnaliseGeralDTO>> result = await _anamneseOrtodonticaAnaliseGeralService.GetAnamneseOrtodonticaAnaliseGeralAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseGeralPaginatedAsync(AnamneseOrtodonticaAnaliseGeralTableFilter filter)
        {
            PaginatedResponse<AnamneseOrtodonticaAnaliseGeralDTO> result = await _anamneseOrtodonticaAnaliseGeralService.GetAnamneseOrtodonticaAnaliseGeralPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseGeralAsync(Guid id)
        {
            Response<AnamneseOrtodonticaAnaliseGeralDTO> result = await _anamneseOrtodonticaAnaliseGeralService.GetAnamneseOrtodonticaAnaliseGeralAsync(id);
            return Ok(result);
        }

        // single by utente 
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseGeralByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOrtodonticaAnaliseGeralDTO?> result = 
                await _anamneseOrtodonticaAnaliseGeralService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOrtodonticaAnaliseGeralAsync([FromBody] CreateAnamneseOrtodonticaAnaliseGeralRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseGeralService.CreateAnamneseOrtodonticaAnaliseGeralAsync(request);
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
        public async Task<IActionResult> UpdateAnamneseOrtodonticaAnaliseGeralAsync([FromBody] UpdateAnamneseOrtodonticaAnaliseGeralRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseGeralService.UpdateAnamneseOrtodonticaAnaliseGeralAsync(request, id);
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
        public async Task<IActionResult> DeleteAnamneseOrtodonticaAnaliseGeralAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOrtodonticaAnaliseGeralService.DeleteAnamneseOrtodonticaAnaliseGeralAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
