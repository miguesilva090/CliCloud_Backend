using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class PromoverAdmissaoResultDTO : IDto
{
  public Guid ConsultaId { get; set; }

  /// <summary>
  /// Legado: TipoAdmiss == 1 (fisioterapia) → HIST_ADF e fluxo de marcações de tratamentos.
  /// </summary>
  public bool SugerirMarcacoesFisio { get; set; }
}
