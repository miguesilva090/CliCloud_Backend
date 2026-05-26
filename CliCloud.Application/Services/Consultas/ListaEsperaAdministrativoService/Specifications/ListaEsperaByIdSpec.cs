using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Specifications;

public sealed class ListaEsperaByIdSpec : Specification<ListaEsperaConsulta>
{
  public ListaEsperaByIdSpec(Guid id)
  {
    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.Organismo)
      .Include(x => x.Prioridade)
      .Include(x => x.TipoConsultaItem)
      .Where(x => x.Id == id && x.DeletedOn == null);
  }
}
