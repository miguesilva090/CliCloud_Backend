using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Application.Services.Utility.TipoCartaService.Specifications
{
  public class TipoCartaSearchTable : Specification<TipoCarta>
  {
    public TipoCartaSearchTable(List<TableFilter> filters, string dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
      {
        foreach (var filter in filters)
        {
          switch (filter.Id.ToLowerInvariant())
          {
            case "descricao":
              if (!string.IsNullOrWhiteSpace(filter.Value))
              {
                _ = Query.Where(x => x.Descricao.Contains(filter.Value));
              }
              break;
            case "obs":
              if (!string.IsNullOrWhiteSpace(filter.Value))
              {
                _ = Query.Where(x => (x.Obs ?? string.Empty).Contains(filter.Value));
              }
              break;
          }
        }
      }

      if (string.IsNullOrWhiteSpace(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Descricao);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}
