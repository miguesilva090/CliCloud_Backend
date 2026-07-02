using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ConfiguracaoADSE;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Specifications
{
  public class ConfiguracaoADSESearchTable : Specification<ConfiguracaoADSE>
  {
    public ConfiguracaoADSESearchTable(string? keyword = "", string? dynamicOrder = "", Guid? empresaId = null)
    {
      if (empresaId.HasValue)
      {
        _ = Query.Where(x => x.EmpresaId == empresaId.Value);
      }

      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x => (x.UrlADSE != null && x.UrlADSE.Contains(keyword)) || (x.Dominio != null && x.Dominio.Contains(keyword)));
      }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.UrlADSE);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}
