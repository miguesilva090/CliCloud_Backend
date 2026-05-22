using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class HorarioMedicoPorMedicoIdSpec : Specification<HorarioMedico>
{
  public HorarioMedicoPorMedicoIdSpec(Guid medicoId)
  {
    _ = Query.Where(h => h.MedicoId == medicoId && h.DeletedOn == null);
  }
}
