using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.Specifications
{
  public class PatologiaSearchTable : Specification<Patologia>
  {
    public PatologiaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
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
            case "organismonome":
              if (!string.IsNullOrWhiteSpace(f.Value))
                _ = Query.Where(x => x.Organismo != null && x.Organismo.Nome != null && x.Organismo.Nome.Contains(f.Value));
              break;
            case "inativo":
              if (!string.IsNullOrWhiteSpace(f.Value) && bool.TryParse(f.Value, out var inativo))
                _ = Query.Where(x => x.Inativo == inativo);
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderBy(x => x.Designacao);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
