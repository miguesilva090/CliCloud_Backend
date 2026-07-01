using Ardalis.Specification;
using CliCloud.Domain.Entities.ConfiguracaoADSE;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Specifications
{
  public class ConfiguracaoADSEPorEmpresaSpec : Specification<ConfiguracaoADSE>
  {
    public ConfiguracaoADSEPorEmpresaSpec(Guid empresaId)
    {
      _ = Query
        .Where(x => x.EmpresaId == empresaId)
        .IgnoreQueryFilters();
    }
  }
}
