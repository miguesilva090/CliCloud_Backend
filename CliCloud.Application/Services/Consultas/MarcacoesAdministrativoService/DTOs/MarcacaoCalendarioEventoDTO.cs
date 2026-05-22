namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoCalendarioEventoDTO
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Start { get; set; } = string.Empty;
  public string End { get; set; } = string.Empty;

  /// <summary>
  /// Marcacao | Feriado | Horario Folga | Indisponível | Horario Variavel | Vagas Extra
  /// </summary>
  public string TipoEvento { get; set; } = string.Empty;

  public Guid? MarcacaoId { get; set; }
  public int? CodigoLegadoTipoConsulta { get; set; }
  public string? SalaCodigo { get; set; }
}
