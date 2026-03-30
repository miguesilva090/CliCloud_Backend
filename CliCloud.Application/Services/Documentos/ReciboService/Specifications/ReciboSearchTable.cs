using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.ReciboService.Specifications
{
  public class ReciboSearchTable : Specification<Recibo>
  {
    public ReciboSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "numerodocumento":
              if (!string.IsNullOrWhiteSpace(f.Value) && int.TryParse(f.Value, out var n)) _ = Query.Where(x => x.NumeroDocumento == n);
              break;
            case "estado":
              if (!string.IsNullOrWhiteSpace(f.Value) && int.TryParse(f.Value, out var e)) _ = Query.Where(x => x.Estado == e);
              break;
            case "liquidado":
              if (!string.IsNullOrWhiteSpace(f.Value) && bool.TryParse(f.Value, out var liq)) _ = Query.Where(x => x.Liquidado == liq);
              break;
          }

      // sort order
      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderByDescending(x => x.Data); // default sort order
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
      }
    }
  }
}
