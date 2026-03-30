using Ardalis.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications
{
  public class AtestadoCategoriaByAtestadoId : Specification<AtestadoCategoria>
  {
    public AtestadoCategoriaByAtestadoId(Guid atestadoId)
    {
      _ = Query.Where(x => x.AtestadoId == atestadoId);
    }
  }
}
