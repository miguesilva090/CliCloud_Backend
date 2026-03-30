using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOrtodonticaAnaliseFuncionalController(IAnamneseOrtodonticaAnaliseFuncionalService anamneseOrtodonticaAnaliseFuncionalService) : ControllerBase
    {
        private readonly IAnamneseOrtodonticaAnaliseFuncionalService _anamneseOrtodonticaAnaliseFuncionalService = anamneseOrtodonticaAnaliseFuncionalService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseFuncionalAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOrtodonticaAnaliseFuncionalDTO>> result = await _anamneseOrtodonticaAnaliseFuncionalService.GetAnamneseOrtodonticaAnaliseFuncionalAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseFuncionalPaginatedAsync(AnamneseOrtodonticaAnaliseFuncionalTableFilter filter)
        {
            PaginatedResponse<AnamneseOrtodonticaAnaliseFuncionalDTO> result = await _anamneseOrtodonticaAnaliseFuncionalService.GetAnamneseOrtodonticaAnaliseFuncionalPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id)
        {
            Response<AnamneseOrtodonticaAnaliseFuncionalDTO> result = await _anamneseOrtodonticaAnaliseFuncionalService.GetAnamneseOrtodonticaAnaliseFuncionalAsync(id);
            return Ok(result);
        }

        //single by utente 
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOrtodonticaAnaliseFuncionalByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOrtodonticaAnaliseFuncionalDTO?> result = await _anamneseOrtodonticaAnaliseFuncionalService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOrtodonticaAnaliseFuncionalAsync(CreateAnamneseOrtodonticaAnaliseFuncionalRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseFuncionalService.CreateAnamneseOrtodonticaAnaliseFuncionalAsync(request);
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
        public async Task<IActionResult> UpdateAnamneseOrtodonticaAnaliseFuncionalAsync(UpdateAnamneseOrtodonticaAnaliseFuncionalRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOrtodonticaAnaliseFuncionalService.UpdateAnamneseOrtodonticaAnaliseFuncionalAsync(request, id);
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
        public async Task<IActionResult> DeleteAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOrtodonticaAnaliseFuncionalService.DeleteAnamneseOrtodonticaAnaliseFuncionalAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
