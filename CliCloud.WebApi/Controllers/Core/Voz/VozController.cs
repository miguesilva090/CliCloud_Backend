using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.VozService;
using CliCloud.Application.Services.Core.VozService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.Voz
{
  [Route("client/core/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class VozController(
    IServicoVoz servicoVoz,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IServicoVoz _servicoVoz = servicoVoz;
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

      var result = await _servicoVoz.ObterConfiguracaoAtualAsync(clinicaId.Value);
      return Ok(result);
    }

    [HttpGet("configuracao/opcoes")]
    public async Task<IActionResult> ObterOpcoesAsync()
    {
      var result = await _servicoVoz.ObterOpcoesAsync();
      return Ok(result);
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync(
      [FromBody] AtualizarConfiguracaoVozRequest request
    )
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoVoz.GuardarConfiguracaoAsync(clinicaId.Value, request);
      return Ok(result);
    }
  }
}
