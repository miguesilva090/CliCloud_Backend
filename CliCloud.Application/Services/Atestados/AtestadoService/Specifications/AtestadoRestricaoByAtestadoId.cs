using Ardalis.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications
{
  public class AtestadoRestricaoByAtestadoId : Specification<AtestadoRestricao>
  {
    public AtestadoRestricaoByAtestadoId(Guid atestadoId)
    {
      _ = Query.Where(x => x.AtestadoId == atestadoId);
    }
  }
}
