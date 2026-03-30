using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Specifications
{
  public class TipoAparelhoSearchTable : Specification<TipoAparelho>
  {
    public TipoAparelhoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
      {
        foreach (var f in filters)
        {
          switch (f.Id.ToLowerInvariant())
          {
            case "designacao":
              if (!string.IsNullOrWhiteSpace(f.Value))
                _ = Query.Where(x => x.Designacao.Contains(f.Value));
              break;
          }
        }
      }

      // sort order
      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Designacao); // default sort order
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
      }
    }
  }
}
