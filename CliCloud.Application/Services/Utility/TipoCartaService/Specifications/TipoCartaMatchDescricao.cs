using Ardalis.Specification;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Application.Services.Utility.TipoCartaService.Specifications
{
  public class TipoCartaMatchDescricao : Specification<TipoCarta>
  {
    public TipoCartaMatchDescricao(string descricao, Guid? exceptId = null)
    {
      _ = Query.Where(x => x.Descricao == descricao);

      if (exceptId.HasValue)
      {
        _ = Query.Where(x => x.Id != exceptId.Value);
      }
    }
  }
}
