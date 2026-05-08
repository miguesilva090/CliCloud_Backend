using Ardalis.Specification;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Application.Services.Utility.TipoCartaService.Specifications
{
  public class TipoCartaSearchList : Specification<TipoCarta>
  {
    public TipoCartaSearchList(string keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x => x.Descricao.Contains(keyword));
      }

      _ = Query.OrderBy(x => x.Descricao);
    }
  }
}
