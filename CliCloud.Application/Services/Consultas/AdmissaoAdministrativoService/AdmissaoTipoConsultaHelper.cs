using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

internal static class AdmissaoTipoConsultaHelper
{
  public static bool EhPrimeiraConsulta(TipoConsultaItem? tipo)
  {
    if (tipo == null)
    {
      return false;
    }

    if (tipo.CodigoLegado == 1)
    {
      return true;
    }

    string d = tipo.Designacao.Trim();
    return d.Contains("1ª", StringComparison.OrdinalIgnoreCase)
      || d.Contains("1a ", StringComparison.OrdinalIgnoreCase)
      || d.StartsWith("1ª", StringComparison.OrdinalIgnoreCase)
      || d.StartsWith("Primeira", StringComparison.OrdinalIgnoreCase)
      || d.StartsWith("1 -", StringComparison.OrdinalIgnoreCase);
  }
}
