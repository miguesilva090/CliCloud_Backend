using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Specifications
{
  public class TipoAparelhoMatchDesignacao : Specification<TipoAparelho>
  {
    public TipoAparelhoMatchDesignacao(string designacao)
    {
      _ = Query.Where(x => x.Designacao == designacao);
    }
  }
}
