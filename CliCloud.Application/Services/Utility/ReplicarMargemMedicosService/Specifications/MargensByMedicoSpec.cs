using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.Specifications;

public class MargensByMedicoSpec : Specification<MargemMedico>
{
    public MargensByMedicoSpec(Guid medicoId)
    {
        Query.Where(x => x.MedicoId == medicoId);
    }
}
