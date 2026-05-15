namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;

/// <summary>Valores de <c>vista</c> alinhados ao submenu legado (Por datas / Por utente / Médicos / Organismos).</summary>
public static class HistoricoConsultaAdministrativoVistas
{
  public const string Datas = "datas";
  public const string Utentes = "utentes";
  public const string Medicos = "medicos";
  public const string Organismos = "organismos";

  public static bool IsValid(string? vista)
  {
    return vista switch
    {
      Datas or Utentes or Medicos or Organismos => true,
      _ => false,
    };
  }
}
