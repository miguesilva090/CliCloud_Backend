using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.ExamesSemPapelService;
using CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[ApiController]
[Route("client/consultas/[controller]")]
[Authorize(Roles = "client")]
public class ExamesSemPapelController(
  IExamesSemPapelService service,
  ICurrentClinicaService currentClinica
) : ControllerBase
{
  private readonly IExamesSemPapelService _service = service;
  private readonly ICurrentClinicaService _currentClinica = currentClinica;

  private async Task<Guid?> ObterClinicaIdAsync()
  {
    await _currentClinica.SetClinicaAsync();
    return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
  }

  [HttpGet("contexto")]
  public async Task<IActionResult> ObterContextoAsync()
  {
    var clinicaId = await ObterClinicaIdAsync();
    if (clinicaId is null) return BadRequest("Clínica atual inválida.");

    return Ok(await _service.ObterContextoAsync(clinicaId.Value));
  }

  [HttpPost("tabela")]
  public async Task<IActionResult> ObterTabelaAsync([FromBody] ExameSemPapelFiltroRequest request)
  {
    var clinicaId = await ObterClinicaIdAsync();
    if (clinicaId is null) return BadRequest("Clínica atual inválida.");

    return Ok(await _service.ObterTabelaAsync(clinicaId.Value, request));
  }

  [HttpPost("assinatura-sessao")]
  public async Task<IActionResult> GuardarAssinaturaSessaoAsync(
    [FromBody] ExameSemPapelAssinaturaSessaoRequest request
  )
  {
    return Ok(await _service.GuardarAssinaturaSessaoAsync(request));
  }

  [HttpDelete("assinatura-sessao")]
  public async Task<IActionResult> LimparAssinaturaSessaoAsync()
  {
    return Ok(await _service.LimparAssinaturaSessaoAsync());
  }

  [HttpPost("assinar-lote")]
  public async Task<IActionResult> AssinarLoteAsync([FromBody] ExameSemPapelLoteRequest request)
  {
    var clinicaId = await ObterClinicaIdAsync();
    if (clinicaId is null) return BadRequest("Clínica atual inválida.");

    return Ok(await _service.AssinarLoteAsync(clinicaId.Value, request));
  }

  [HttpPost("comunicar-lote")]
  public async Task<IActionResult> ComunicarLoteAsync([FromBody] ExameSemPapelLoteRequest request)
  {
    var clinicaId = await ObterClinicaIdAsync();
    if (clinicaId is null) return BadRequest("Clínica atual inválida.");

    return Ok(await _service.ComunicarLoteAsync(clinicaId.Value, request));
  }
}
