using System.Globalization;

namespace CliCloud.Application.Services.Consultas.ConsultaService;

/// <summary>
/// Conversão entre horas da API (string, legado) e <see cref="TimeSpan"/> na entidade / SQL <c>time</c>.
/// </summary>
public static class ConsultaHoraMapper
{
  public static TimeSpan? ParseFromApi(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return null;
    }

    string trimmed = value.Trim();
    if (TimeSpan.TryParse(trimmed, CultureInfo.InvariantCulture, out TimeSpan parsed))
    {
      return parsed;
    }

    if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
    {
      return dt.TimeOfDay;
    }

    return null;
  }

  public static string? FormatForApi(TimeSpan? value) =>
    value.HasValue ? value.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null;
}
