using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.Specifications
{
  public class PatologiaSearchList : Specification<Patologia>
  {
    public PatologiaSearchList(string keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x => x.Designacao.Contains(keyword));
      _ = Query.OrderBy(x => x.Designacao);
    }
  }
}
