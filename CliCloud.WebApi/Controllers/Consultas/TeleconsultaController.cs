using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.TeleconsultaService;
using CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas
{
  [Route("client/consultas/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class TeleconsultaController(
    IServicoTeleconsulta servicoTeleconsulta,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IServicoTeleconsulta _servicoTeleconsulta = servicoTeleconsulta;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
      await _currentClinica.SetClinicaAsync();
      if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId))
        return null;

      return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [HttpPost("sessao")]
    public async Task<IActionResult> CriarOuObterSessaoAsync([FromBody] CriarTeleconsultaRequest request)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.CriarOuObterSessaoAsync(clinicaId.Value, request);
      return Ok(result);
    }

    [HttpGet("sessao/marcacao/{consultaMarcacaoId:guid}")]
    public async Task<IActionResult> ObterPorMarcacaoAsync(Guid consultaMarcacaoId)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.ObterPorMarcacaoAsync(clinicaId.Value, consultaMarcacaoId);
      return Ok(result);
    }

    [HttpPost("sessao/{sessaoId:guid}/entrar")]
    public async Task<IActionResult> ObterLinkEntradaAsync(Guid sessaoId, [FromBody] EntrarTeleconsultaRequest request)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.ObterLinkEntradaAsync(clinicaId.Value, sessaoId, request);
      return Ok(result);
    }

    [HttpPost("sessao/{sessaoId:guid}/link-utente")]
    public async Task<IActionResult> GerarLinkUtenteAsync(
      Guid sessaoId,
      [FromBody] GerarLinkUtenteRequest request
    )
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.GerarLinkUtenteAsync(clinicaId.Value, sessaoId, request);
      return Ok(result);
    }

    [HttpPost("sessao/{sessaoId:guid}/revogar-links")]
    public async Task<IActionResult> RevogarLinksSessaoAsync(Guid sessaoId)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.RevogarLinksSessaoAsync(clinicaId.Value, sessaoId);
      return Ok(result);
    }

    [HttpPost("sessao/{sessaoId:guid}/terminar")]
    public async Task<IActionResult> TerminarSessaoAsync(Guid sessaoId)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _servicoTeleconsulta.TerminarSessaoAsync(clinicaId.Value, sessaoId);
      return Ok(result);
    }
  }
}
