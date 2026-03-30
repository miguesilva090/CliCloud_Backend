using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.Specifications
{
  public class AparelhoSearchTable : Specification<Aparelho>
  {
    public AparelhoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      _ = Query.Include(x => x.TipoAparelho);
      _ = Query.Include(x => x.ModeloAparelho).ThenInclude(m => m!.MarcaAparelho);
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "codigoserie":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.CodigoSerie != null && x.CodigoSerie.Contains(f.Value));
              break;
            case "local":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Local != null && x.Local.Contains(f.Value));
              break;
            case "tipoaparelholdesignacao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.TipoAparelho != null && x.TipoAparelho.Designacao.Contains(f.Value));
              break;
            case "modeloaparelholdesignacao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.ModeloAparelho != null && x.ModeloAparelho.Designacao.Contains(f.Value));
              break;
            case "marcaaparelholdesignacao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.ModeloAparelho != null && x.ModeloAparelho.MarcaAparelho != null && x.ModeloAparelho.MarcaAparelho.Designacao.Contains(f.Value));
              break;
            case "ocupado":
              if (!string.IsNullOrWhiteSpace(f.Value) && bool.TryParse(f.Value, out var oc)) _ = Query.Where(x => x.Ocupado == oc);
              break;
          }

      // sort order
      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.CodigoSerie); // default sort order
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
      }
    }
  }
}
