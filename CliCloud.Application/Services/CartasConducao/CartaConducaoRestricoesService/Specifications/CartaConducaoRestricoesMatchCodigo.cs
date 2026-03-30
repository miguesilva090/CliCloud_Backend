using Ardalis.Specification;
using CliCloud.Domain.Entities.CartaConducao;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Specifications
{
  public class CartaConducaoRestricoesMatchCodigo : Specification<CartaConducaoRestricao>
  {
    public CartaConducaoRestricoesMatchCodigo(int codigoRestricao)
    {
      _ = Query.Where(x => x.CodigoRestricao == codigoRestricao);
      _ = Query.OrderBy(x => x.CodigoRestricao);
    }
  }
}
