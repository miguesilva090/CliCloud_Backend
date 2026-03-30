using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.Specifications
{
    public class HorarioMedicoVariavelSearchByMedicoId : Specification<HorarioMedicoVariavel>
    {
        public HorarioMedicoVariavelSearchByMedicoId(Guid medicoId)
        {
            _ = Query.Where(x => x.MedicoId == medicoId);
            _ = Query.Include(x => x.Medico);
            _ = Query.OrderBy(x => x.Data);
        }
    }
}
