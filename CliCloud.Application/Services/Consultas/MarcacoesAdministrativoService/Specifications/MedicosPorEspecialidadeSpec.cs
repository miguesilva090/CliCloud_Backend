using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;

public class MedicosPorEspecialidadeSpec : Specification<Medico>
{
  public MedicosPorEspecialidadeSpec(Guid especialidadeId)
  {
    _ = Query.Where(m => m.EspecialidadeId == especialidadeId);
    _ = Query.OrderBy(m => m.Nome);
  }
}
