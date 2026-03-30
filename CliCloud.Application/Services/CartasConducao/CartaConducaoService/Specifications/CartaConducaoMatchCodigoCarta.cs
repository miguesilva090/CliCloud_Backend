using Ardalis.Specification;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.Specifications
{
  public class CartaConducaoMatchCodigoCarta : Specification<CartaConducaoEntity>
  {
    public CartaConducaoMatchCodigoCarta(string? codigoCarta)
    {
      if (!string.IsNullOrWhiteSpace(codigoCarta))
        _ = Query.Where(x => x.CodigoCarta == codigoCarta);
      _ = Query.OrderBy(x => x.CodigoCarta);
    }
  }
}
