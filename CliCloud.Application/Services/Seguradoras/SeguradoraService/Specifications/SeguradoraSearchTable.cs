using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Seguradoras;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.Specifications
{
  public class SeguradoraSearchTable : Specification<Seguradora>
  {
    public SeguradoraSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "nome":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Nome.Contains(f.Value));
              break;
            case "apolice":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Apolice != null && x.Apolice.Contains(f.Value));
              break;
            case "abreviatura":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Abreviatura != null && x.Abreviatura.Contains(f.Value));
              break;
          }

      // sort order
      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Nome); // default sort order
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
      }
    }
  }
}
