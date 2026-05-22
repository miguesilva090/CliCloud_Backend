using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;

public sealed class MarcacaoAdministrativoByIdSpec : Specification<ConsultaMarcacao>
{
  public MarcacaoAdministrativoByIdSpec(Guid id)
  {
    _ = Query
      .Where(x => x.Id == id)
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.Sala)
      .Include(x => x.TipoConsultaItem);
  }
}
