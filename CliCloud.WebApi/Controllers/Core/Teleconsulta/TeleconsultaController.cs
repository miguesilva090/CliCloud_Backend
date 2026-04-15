using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.TeleconsultaService;
using CliCloud.Application.Services.Core.TeleconsultaService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.Teleconsulta
{
  [Route("client/core/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class TeleconsultaController(
    IConfiguracaoTeleconsultaService configuracaoTeleconsultaService,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IConfiguracaoTeleconsultaService _configuracaoTeleconsultaService = configuracaoTeleconsultaService;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
      await _currentClinica.SetClinicaAsync();
      if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId))
        return null;

      return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [HttpGet("configuracao")]
    public async Task<IActionResult> ObterConfiguracaoAtualAsync()
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _configuracaoTeleconsultaService.ObterConfiguracaoAtualAsync(clinicaId.Value);
      return Ok(result);
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync(
      [FromBody] AtualizarConfiguracaoTeleconsultaRequest request
    )
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _configuracaoTeleconsultaService.GuardarConfiguracaoAsync(clinicaId.Value, request);
      return Ok(result);
    }
  }
}
