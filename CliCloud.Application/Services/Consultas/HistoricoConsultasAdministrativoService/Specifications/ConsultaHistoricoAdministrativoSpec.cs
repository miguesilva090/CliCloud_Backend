using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Specifications;

/// <summary>
/// Filtro por id. GET usa <c>ProjectTo</c> (sem includes). Serviços em query separada.
/// </summary>
public sealed class ConsultaHistoricoAdministrativoSpec : Specification<Consulta>
{
  public ConsultaHistoricoAdministrativoSpec(Guid id)
  {
    _ = Query.Where(x => x.Id == id && x.DeletedOn == null);
  }
}
