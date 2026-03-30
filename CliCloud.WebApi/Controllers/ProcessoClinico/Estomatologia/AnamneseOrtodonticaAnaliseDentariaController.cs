using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOrtodonticaAnaliseDentariaController(IAnamneseOrtodonticaAnaliseDentariaService anamneseOrtodonticaAnaliseDentariaService) : ControllerBase
    {
        private readonly IAnamneseOrtodonticaAnaliseDentariaService _anamneseOrtodonticaAnaliseDentariaService = anamneseOrtodonticaAnaliseDentariaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseDentariaAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOrtodonticaAnaliseDentariaDTO>> result = await _anamneseOrtodonticaAnaliseDentariaService.GetAnamneseOrtodonticaAnaliseDentariaAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseDentariaPaginatedAsync(AnamneseOrtodonticaAnaliseDentariaTableFilter filter)
        {
            PaginatedResponse<AnamneseOrtodonticaAnaliseDentariaDTO> result = await _anamneseOrtodonticaAnaliseDentariaService.GetAnamneseOrtodonticaAnaliseDentariaPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseDentariaAsync(Guid id)
        {
            Response<AnamneseOrtodonticaAnaliseDentariaDTO> result = await _anamneseOrtodonticaAnaliseDentariaService.GetAnamneseOrtodonticaAnaliseDentariaAsync(id);
            return Ok(result);
        }

        // single by utente 
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseDentariaByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOrtodonticaAnaliseDentariaDTO?> result = 
                await _anamneseOrtodonticaAnaliseDentariaService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOrtodonticaAnaliseDentariaAsync(CreateAnamneseOrtodonticaAnaliseDentariaRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseDentariaService.CreateAnamneseOrtodonticaAnaliseDentariaAsync(request);
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
        public async Task<IActionResult> UpdateAnamneseOrtodonticaAnaliseDentariaAsync(UpdateAnamneseOrtodonticaAnaliseDentariaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseDentariaService.UpdateAnamneseOrtodonticaAnaliseDentariaAsync(request, id);
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
        public async Task<IActionResult> DeleteAnamneseOrtodonticaAnaliseDentariaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOrtodonticaAnaliseDentariaService.DeleteAnamneseOrtodonticaAnaliseDentariaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
