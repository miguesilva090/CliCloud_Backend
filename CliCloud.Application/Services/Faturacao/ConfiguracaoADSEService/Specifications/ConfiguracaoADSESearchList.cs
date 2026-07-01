using Ardalis.Specification;
using CliCloud.Domain.Entities.ConfiguracaoADSE;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Specifications
{
  public class ConfiguracaoADSESearchList : Specification<ConfiguracaoADSE>
  {
    public ConfiguracaoADSESearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x => (x.UrlADSE != null && x.UrlADSE.Contains(keyword)) || (x.Dominio != null && x.Dominio.Contains(keyword)));
      }
      _ = Query.OrderBy(x => x.CreatedOn).ThenBy(x => x.Id);
    }
  }
}
