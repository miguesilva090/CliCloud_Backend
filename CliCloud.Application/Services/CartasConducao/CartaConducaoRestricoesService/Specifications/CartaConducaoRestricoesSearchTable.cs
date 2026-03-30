using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Specifications
{
  public class CartaConducaoRestricoesSearchTable : Specification<CartaConducaoRestricao>
  {
    public CartaConducaoRestricoesSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
      {
        foreach (var f in filters)
        {
          switch (f.Id.ToLowerInvariant())
          {
            case "codigorestricao":
              if (int.TryParse(f.Value, out var cod)) _ = Query.Where(x => x.CodigoRestricao == cod);
              break;
            case "descricao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Descricao != null && x.Descricao.Contains(f.Value));
              break;
            case "inativo":
              if (bool.TryParse(f.Value, out var inativo)) _ = Query.Where(x => x.Inativo == inativo);
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderBy(x => x.CodigoRestricao);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
