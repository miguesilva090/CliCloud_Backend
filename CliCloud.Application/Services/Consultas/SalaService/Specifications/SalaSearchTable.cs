using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.SalaService.Specifications
{
  public class SalaSearchTable : Specification<Sala>
  {
    public SalaSearchTable(List<TableFilter> filters, string dynamicOrder = "")
    {
      _ = Query.Include(x => x.Clinica);

      if (filters != null && filters.Count > 0)
      {
        foreach (var filter in filters)
        {
          switch (filter.Id.ToLowerInvariant())
          {
            case "nome":
              if (!string.IsNullOrWhiteSpace(filter.Value))
              {
                _ = Query.Where(x => x.Nome.Contains(filter.Value));
              }
              break;
            case "numerosala":
              if (int.TryParse(filter.Value, out var numeroSala))
              {
                _ = Query.Where(x => x.NumeroSala == numeroSala);
              }
              break;
            case "ativa":
              if (bool.TryParse(filter.Value, out var ativa))
              {
                _ = Query.Where(x => x.Ativa == ativa);
              }
              break;
            case "clinicaid":
              if (Guid.TryParse(filter.Value, out var clinicaId))
              {
                _ = Query.Where(x => x.ClinicaId == clinicaId);
              }
              break;
          }
        }
      }

      if (string.IsNullOrWhiteSpace(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Nome);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}
