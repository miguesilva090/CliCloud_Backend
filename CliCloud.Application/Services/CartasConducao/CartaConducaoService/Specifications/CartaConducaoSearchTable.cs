using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.Specifications
{
  public class CartaConducaoSearchTable : Specification<CartaConducaoEntity>
  {
    public CartaConducaoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
      {
        foreach (var f in filters)
        {
          switch (f.Id.ToLowerInvariant())
          {
            case "codigocarta":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.CodigoCarta != null && x.CodigoCarta.Contains(f.Value));
              break;
            case "descricao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Descricao != null && x.Descricao.Contains(f.Value));
              break;
            case "grupo":
              if (int.TryParse(f.Value, out var grupo)) _ = Query.Where(x => x.Grupo == grupo);
              break;
            case "inativo":
              if (bool.TryParse(f.Value, out var inativo)) _ = Query.Where(x => x.Inativo == inativo);
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderBy(x => x.Grupo).ThenBy(x => x.CodigoCarta);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
