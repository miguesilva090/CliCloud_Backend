using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/[controller]")]
[ApiController]
public class DisponibilidadeTecnicoTratamentoController(
  IDisponibilidadeTecnicoTratamentoService service
) : ControllerBase
{
  private readonly IDisponibilidadeTecnicoTratamentoService _service = service;

  /// <summary>Legado: GetMaximoTratamentos* → [1..maxtrat].</summary>
  [Authorize(Roles = "client")]
  [HttpGet("unidades-tempo/{tecnicoId:guid}")]
  public async Task<IActionResult> GetUnidadesTempoAsync(Guid tecnicoId)
  {
    Response<UnidadesTempoTecnicoResponse> result =
      await _service.GetUnidadesTempoAsync(tecnicoId);
    return Ok(result);
  }

  /// <summary>
  /// Legado: ObterHorasPossiveisDia* (+ ClinFolg / feriado).
  /// Em folga clínica ou feriado: Response Failure com mensagem (ex. "Dia de Folga Clínica").
  /// </summary>
  [Authorize(Roles = "client")]
  [HttpPost("horas-possiveis")]
  public async Task<IActionResult> GetHorasPossiveisAsync(
    [FromBody] HorasPossiveisTecnicoRequest request
  )
  {
    Response<HorasPossiveisTecnicoResponse> result =
      await _service.GetHorasPossiveisAsync(request);
    return Ok(result);
  }
}
