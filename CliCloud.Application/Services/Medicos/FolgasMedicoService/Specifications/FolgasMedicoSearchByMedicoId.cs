using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.FolgasMedicoService.Specifications
{
    public class FolgasMedicoSearchByMedicoId : Specification<FolgasMedico>
    {
        public FolgasMedicoSearchByMedicoId(Guid medicoId)
        {
            _ = Query.Where(x => x.MedicoId == medicoId);
            _ = Query.Include(x => x.Medico);
            _ = Query.OrderBy(x => x.DataDe).ThenBy(x => x.DataAte);
        }
    }
}
