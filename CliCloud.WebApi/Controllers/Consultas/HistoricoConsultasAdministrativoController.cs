using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/historico-administrativo")]
[ApiController]
public class HistoricoConsultasAdministrativoController(IHistoricoConsultasAdministrativoService service)
  : ControllerBase
{
  private readonly IHistoricoConsultasAdministrativoService _service = service;

  /// <summary>Listagem paginada do histórico administrativo (vista: datas, utentes, medicos, organismos).</summary>
  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginatedAsync([FromBody] HistoricoConsultaAdministrativoTableFilter filter)
  {
    string? v = filter.Vista?.Trim().ToLowerInvariant();
    if (!string.IsNullOrEmpty(v) && !HistoricoConsultaAdministrativoVistas.IsValid(v))
    {
      return BadRequest(
        $"Vista inválida: '{filter.Vista}'. Use: {HistoricoConsultaAdministrativoVistas.Datas}, "
          + $"{HistoricoConsultaAdministrativoVistas.Utentes}, {HistoricoConsultaAdministrativoVistas.Medicos} ou "
          + $"{HistoricoConsultaAdministrativoVistas.Organismos}.");
    }

    PaginatedResponse<HistoricoConsultaAdministrativoRowDTO> result = await _service.GetPaginatedAsync(filter);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetConsultaForEdit(Guid id)
    => Ok(await _service.GetConsultaHistoricoForEditAsync(id));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpdateConsultaHistorico(Guid id, [FromBody] UpdateConsultaHistoricoRequest request)
    => Ok(await _service.UpdateConsultaHistoricoAsync(id, request));

}
