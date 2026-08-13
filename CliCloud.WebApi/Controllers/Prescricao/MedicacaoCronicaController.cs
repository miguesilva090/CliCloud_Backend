using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.MedicacaoCronicaService;
using CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.DTOs;

namespace CliCloud.WebApi.Controllers.Prescricao
{
  [Route("client/prescricao/[controller]")]
  [ApiController]
  public class MedicacaoCronicaController(IMedicacaoCronicaService service) : ControllerBase
  {
    [Authorize(Roles = "client")]
    [HttpGet("by-utente/{utenteId:guid}")]
    public async Task<IActionResult> GetByUtenteIdAsync(
      Guid utenteId, [FromQuery] bool apenasAtivos = true)
    {
      Response<IEnumerable<MedicacaoCronicaDTO>> result =
        await service.GetByUtenteIdAsync(utenteId, apenasAtivos);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateMedicacaoCronicaRequest request)
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
