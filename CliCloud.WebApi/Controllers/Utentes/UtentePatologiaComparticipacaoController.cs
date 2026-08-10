using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService;
using CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utentes
{
    [Route("client/utentes/[controller]")]
    [ApiController]
    public class UtentePatologiaComparticipacaoController(
        IUtentePatologiaComparticipacaoService service) : ControllerBase
    {
        [Authorize(Roles = "client")]
        [HttpGet("by-utente/{utenteId:guid}")]
        public async Task<IActionResult> GetByUtenteIdAsync(Guid utenteId)
        {
            Response<IEnumerable<UtentePatologiaComparticipacaoDTO>> result =
                await service.GetByUtenteIdAsync(utenteId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateUtentePatologiaComparticipacaoRequest request)
        {
            Response<Guid> result = await service.CreateAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPut("by-utente/{utenteId:guid}")]
        public async Task<IActionResult> ReplaceByUtenteAsync(
            Guid utenteId,
            [FromBody] ReplaceUtentePatologiasComparticipacaoRequest request)
        {
            request.UtenteId = utenteId;
            Response<IEnumerable<UtentePatologiaComparticipacaoDTO>> result =
                await service.ReplaceByUtenteAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            Response<Guid> result = await service.DeleteAsync(id);
            return Ok(result);
        }
    }
}