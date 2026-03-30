using System.Globalization;
using Ardalis.Specification;
using CliCloud.Domain.Entities.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Specifications
{
  public class CartaConducaoRestricoesSearchList : Specification<CartaConducaoRestricao>
  {
    public CartaConducaoRestricoesSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x =>
          (x.Descricao != null && x.Descricao.Contains(keyword)) ||
          x.CodigoRestricao.ToString(CultureInfo.InvariantCulture).Contains(keyword));
      }
      _ = Query.OrderBy(x => x.CodigoRestricao);
    }
  }
}
