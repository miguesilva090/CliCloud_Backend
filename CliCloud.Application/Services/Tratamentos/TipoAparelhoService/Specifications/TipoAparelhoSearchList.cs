using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Specifications
{
  public class TipoAparelhoSearchList : Specification<TipoAparelho>
  {
    public TipoAparelhoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x => x.Designacao.Contains(keyword));
      _ = Query.OrderBy(x => x.Designacao);
    }
  }
}
