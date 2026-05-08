using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.Specifications
{
  public class MotivoConsultaSearchTable : Specification<MotivoConsulta>
  {
    public MotivoConsultaSearchTable(List<TableFilter> filters, string dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
      {
        foreach (var filter in filters)
        {
          switch (filter.Id.ToLowerInvariant())
          {
            case "designacao":
              if (!string.IsNullOrWhiteSpace(filter.Value))
              {
                _ = Query.Where(x => x.Designacao.Contains(filter.Value));
              }
              break;
          }
        }
      }

      if (string.IsNullOrWhiteSpace(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Designacao);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}
