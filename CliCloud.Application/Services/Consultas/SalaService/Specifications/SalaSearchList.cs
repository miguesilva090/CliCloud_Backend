using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.SalaService.Specifications
{
  public class SalaSearchList : Specification<Sala>
  {
    public SalaSearchList(string keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x => x.Nome.Contains(keyword));
      }

      _ = Query.OrderBy(x => x.Nome);
    }
  }
}
