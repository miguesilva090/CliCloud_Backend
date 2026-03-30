using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  public class ClinicaSearchTable : Specification<Clinica>
  {
    public ClinicaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "nome":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Nome.Contains(f.Value));
              break;
            case "nomecomercial":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.NomeComercial != null && x.NomeComercial.Contains(f.Value));
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
