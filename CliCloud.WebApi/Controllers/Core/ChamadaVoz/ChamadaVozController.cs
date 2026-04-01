using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ChamadaVozService;
using CliCloud.Application.Services.Core.ChamadaVozService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.ChamadaVoz
{
  [Route("client/core/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class ChamadaVozController(
    IServicoChamadaVoz servicoChamadaVoz,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IServicoChamadaVoz _servicoChamadaVoz = servicoChamadaVoz;
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

      var result = await _servicoChamadaVoz.ObterConfiguracaoAtualAsync(clinicaId.Value);
      return Ok(result);
    }

    [HttpGet("configuracao/opcoes")]
    public async Task<IActionResult> ObterOpcoesAsync()
    {
      var result = await _servicoChamadaVoz.ObterOpcoesAsync();
      return Ok(result);
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync(
      [FromBody] AtualizarConfiguracaoChamadaVozRequest request
    )
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoChamadaVoz.GuardarConfiguracaoAsync(clinicaId.Value, request);
      return Ok(result);
    }
  }
}
