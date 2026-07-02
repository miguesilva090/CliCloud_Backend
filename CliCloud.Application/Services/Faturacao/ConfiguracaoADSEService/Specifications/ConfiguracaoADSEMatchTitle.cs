using Ardalis.Specification;
using CliCloud.Domain.Entities.ConfiguracaoADSE;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Specifications
{
  public class ConfiguracaoADSEMatchTitle : Specification<ConfiguracaoADSE>
  {
    public ConfiguracaoADSEMatchTitle(string? UrlADSE)
    {
      if (!string.IsNullOrWhiteSpace(UrlADSE))
      {
        _ = Query.Where(h => h.UrlADSE == UrlADSE);
      }
      _ = Query.OrderBy(h => h.UrlADSE);
    }
  }
}
