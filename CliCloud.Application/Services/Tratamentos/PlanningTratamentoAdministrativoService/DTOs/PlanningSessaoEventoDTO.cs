namespace CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.DTOs;

public class PlanningSessaoEventoDTO
{
  public Guid SessaoId { get; set; }
  public Guid TratamentoId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Start { get; set; } = string.Empty;
  public string End { get; set; } = string.Empty;
  /// <summary>
  /// 1=1ª sessão, 2=marcada, 3=última, 4=última+alta, 5=provisório, 6=falta, 11=multi-técnico.
  /// </summary>
  public int TipoEvento { get; set; }
  public int? NumSessao { get; set; }
  public string? HoraInicio { get; set; }
  public string? Duracao { get; set; }
  public string? UtenteNome { get; set; }
  public string? TratamentoDesignacao { get; set; }
  public int? NumSessoesTratamento { get; set; }
  public int? NFaltas { get; set; }
  public DateTime? DataInicTratamento { get; set; }
  public DateTime? DataFimTratamento { get; set; }
  public bool Faltou { get; set; }
  public bool Confirmado { get; set; }
  public bool Efetuado { get; set; }
}
