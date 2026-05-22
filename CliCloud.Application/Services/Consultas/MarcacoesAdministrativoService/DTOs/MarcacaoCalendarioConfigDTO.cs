namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoCalendarioConfigDTO
{
  /// <summary>Intervalo entre slots (legado intervalo_marcacao / slotDuration).</summary>
  public string IntervaloMarcacao { get; set; } = "00:30:00";

  /// <summary>Duração 1ª consulta quando aplicável (legado primeira_consulta).</summary>
  public string? PrimeiraConsulta { get; set; }

  /// <summary>Início visível do calendário (legado limite_manha).</summary>
  public string LimiteManha { get; set; } = "08:00";

  /// <summary>Fim visível do calendário (legado limite_tarde).</summary>
  public string LimiteTarde { get; set; } = "20:00";

  /// <summary>Dias da semana visíveis no calendário (FullCalendar: 0=Dom … 6=Sáb).</summary>
  public List<int> DiasUteis { get; set; } = [1, 2, 3, 4, 5];

  /// <summary>Dias ocultos no calendário (legado hiddenDays após GetDiasFolgaClinica).</summary>
  public List<int> DiasOcultos { get; set; } = [];
}
