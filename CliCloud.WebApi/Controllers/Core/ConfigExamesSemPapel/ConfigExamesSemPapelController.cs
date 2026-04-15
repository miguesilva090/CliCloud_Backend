using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.ConfigExamesSemPapel;

[ApiController]
[Route("client/core/[controller]")]
[Authorize(Roles = "client")]
public class ConfigExamesSemPapelController(
    IConfigExamesSemPapelService service,
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly IConfigExamesSemPapelService _service = service;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinica.SetClinicaAsync();
        return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [HttpGet("configuracao")]
    public async Task<IActionResult> ObterConfiguracaoAtualAsync()
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.ObterConfiguracaoAtualAsync(clinicaId.Value));
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarConfigExamesSemPapelRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.GuardarConfiguracaoAsync(clinicaId.Value, request));
    }
}
