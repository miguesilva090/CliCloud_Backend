using Ardalis.Specification;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.Specifications
{
  public class CartaConducaoSearchList : Specification<CartaConducaoEntity>
  {
    public CartaConducaoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x =>
          (x.CodigoCarta != null && x.CodigoCarta.Contains(keyword)) ||
          (x.Descricao != null && x.Descricao.Contains(keyword)));
      }
      _ = Query.OrderBy(x => x.Grupo).ThenBy(x => x.CodigoCarta);
    }
  }
}
