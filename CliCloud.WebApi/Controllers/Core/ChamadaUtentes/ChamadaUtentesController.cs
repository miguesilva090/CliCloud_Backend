using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ChamadaUtentesService;
using CliCloud.Application.Services.Core.ChamadaUtentesService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.ChamadaUtentes
{
  [Route("client/core/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class ChamadaUtentesController(
    IChamadaUtentesService chamadaUtentesService,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IChamadaUtentesService _chamadaUtentesService = chamadaUtentesService;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
      await _currentClinica.SetClinicaAsync();
      if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId))
        return null;

      return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [HttpGet("consultas/{marcacaoConsultaId:guid}/dados")]
    public async Task<IActionResult> ObterDadosChamadaConsultaAsync(Guid marcacaoConsultaId, [FromQuery] bool chamarOutraVez = false)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.ObterDadosChamadaConsultaAsync(clinicaId.Value, marcacaoConsultaId, chamarOutraVez);
      return Ok(result);
    }

    [HttpPost("consultas/{marcacaoConsultaId:guid}/chamar")]
    public async Task<IActionResult> ChamarUtenteConsultaAsync(Guid marcacaoConsultaId, [FromBody] ChamarConsultaRequest request)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.ChamarUtenteConsultaAsync(clinicaId.Value, marcacaoConsultaId, request);
      return Ok(result);
    }

    [HttpGet("tratamentos/{sessaoTratamentoId:guid}/dados")]
    public async Task<IActionResult> ObterDadosChamadaTratamentoAsync(Guid sessaoTratamentoId, [FromQuery] bool chamarOutraVez = false)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.ObterDadosChamadaTratamentoAsync(clinicaId.Value, sessaoTratamentoId, chamarOutraVez);
      return Ok(result);
    }

    [HttpPost("tratamentos/{sessaoTratamentoId:guid}/chamar")]
    public async Task<IActionResult> ChamarUtenteTratamentoAsync(Guid sessaoTratamentoId, [FromBody] ChamarSessaoTratamentoRequest request)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.ChamarUtenteTratamentoAsync(clinicaId.Value, sessaoTratamentoId, request);
      return Ok(result);
    }

    [HttpGet("fila")]
    public async Task<IActionResult> ObterFilaAsync([FromQuery] string? tipo = null)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.ObterChamadasAsync(clinicaId.Value, tipo);
      return Ok(result);
    }

    [HttpPut("{chamadaId:guid}/estado")]
    public async Task<IActionResult> AtualizarEstadoAsync(Guid chamadaId, [FromBody] AtualizarEstadoChamadaRequest request)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");

      var result = await _chamadaUtentesService.AtualizarEstadoAsync(clinicaId.Value, chamadaId, request.Estado);
      return Ok(result);
    }
  }
}
