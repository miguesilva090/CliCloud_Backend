using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico;

[Route("client/processo-clinico/[controller]")]
[ApiController]
public class SeparadorPersonalizadoVinculoController(
    ISeparadorPersonalizadoVinculoService service,
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly ISeparadorPersonalizadoVinculoService _service = service;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinica.SetClinicaAsync();
        return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [Authorize(Roles = "client")]
    [HttpGet("separador/{separadorPersonalizadoId:guid}")]
    public async Task<IActionResult> GetBySeparadorAsync(Guid separadorPersonalizadoId)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<IEnumerable<SeparadorPersonalizadoVinculoDTO>> result =
            await _service.GetBySeparadorAsync(clinicaId.Value, separadorPersonalizadoId);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateSeparadorPersonalizadoVinculoRequest request
    )
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<Guid> result = await _service.CreateAsync(clinicaId.Value, request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<Guid> result = await _service.DeleteAsync(clinicaId.Value, id);
        return Ok(result);
    }
}
