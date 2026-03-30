using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications
{
    public class HorarioMedicoSearchByMedicoId : Specification<HorarioMedico>
    {
        public HorarioMedicoSearchByMedicoId(Guid medicoId)
        {
            _ = Query.Where(x => x.MedicoId == medicoId);
            _ = Query.Include(x => x.Medico)
                .Include(x => x.Horarios);
            _ = Query.OrderBy(x => x.CreatedOn);
        }
    }
}
