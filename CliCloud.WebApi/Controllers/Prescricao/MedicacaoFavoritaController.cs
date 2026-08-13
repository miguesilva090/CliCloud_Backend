using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService;
using CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.DTOs;

namespace CliCloud.WebApi.Controllers.Prescricao
{
    [Route("client/prescricao/[controller]")]
    [ApiController]
    public class MedicacaoFavoritaController(IMedicacaoFavoritaService service) : ControllerBase
    {
        [Authorize(Roles = "client")]
        [HttpGet("by-medico/{medicoId:guid}")]
        public async Task<IActionResult> getByMedicoIdAsync(Guid medicoId, [FromQuery] int? tipoLinha = null)
        {
            Response<IEnumerable<MedicacaoFavoritaDTO>> result = 
                await service.GetByMedicoIdAsync(medicoId, tipoLinha);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateMedicacaoFavoritaRequest request
        )
        {
            Response<Guid> result = await service.CreateAsync(request);
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