using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.ServicoService.Specifications
{
  public class ServicoSearchTable : Specification<Servico>
  {
    public ServicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "designacao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Designacao.Contains(f.Value));
              break;
            case "tiposervicoid":
              if (Guid.TryParse(f.Value, out var tsId)) _ = Query.Where(x => x.TipoServicoId == tsId);
              break;
            case "inativo":
              if (bool.TryParse(f.Value, out var b)) _ = Query.Where(x => x.Inativo == b);
              break;
          }

      if (string.IsNullOrEmpty(dynamicOrder))
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

