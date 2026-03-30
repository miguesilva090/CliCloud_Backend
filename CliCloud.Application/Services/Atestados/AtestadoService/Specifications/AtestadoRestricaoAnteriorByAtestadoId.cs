using Ardalis.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications
{
  public class AtestadoRestricaoAnteriorByAtestadoId : Specification<AtestadoRestricaoAnterior>
  {
    public AtestadoRestricaoAnteriorByAtestadoId(Guid atestadoId)
    {
      _ = Query.Where(x => x.AtestadoId == atestadoId);
    }
  }
}
