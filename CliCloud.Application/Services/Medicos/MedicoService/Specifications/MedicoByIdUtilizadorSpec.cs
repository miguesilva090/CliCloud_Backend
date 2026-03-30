using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MedicoService.Specifications
{
  public class MedicoByIdUtilizadorSpec : Specification<Medico>
  {
    public MedicoByIdUtilizadorSpec(Guid idUtilizador)
    {
      _ = Query.Where(m => m.IdUtilizador == idUtilizador);
    }
  }
}
