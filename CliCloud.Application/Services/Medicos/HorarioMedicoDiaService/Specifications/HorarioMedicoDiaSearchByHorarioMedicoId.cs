using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Specifications
{
    public class HorarioMedicoDiaSearchByHorarioMedicoId : Specification<HorarioMedicoDia>
    {
        public HorarioMedicoDiaSearchByHorarioMedicoId(Guid horarioMedicoId)
        {
            _ = Query.Where(x => x.HorarioMedicoId == horarioMedicoId);
            _ = Query.Include(x => x.HorarioMedico)
                .ThenInclude(x => x.Medico);
            _ = Query.OrderBy(x => x.DiaSemana)
                .ThenBy(x => x.Periodo)
                .ThenBy(x => x.Inicio);
        }
    }
}
