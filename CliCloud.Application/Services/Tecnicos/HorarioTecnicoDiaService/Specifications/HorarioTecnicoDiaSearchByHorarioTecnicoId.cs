using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Specifications
{
    public class HorarioTecnicoDiaSearchByHorarioTecnicoId : Specification<HorarioTecnicoDia>
    {
        public HorarioTecnicoDiaSearchByHorarioTecnicoId(Guid horarioTecnicoId)
        {
            _ = Query.Where(x => x.HorarioTecnicoId == horarioTecnicoId);
            _ = Query.Include(x => x.HorarioTecnico)
                .ThenInclude(x => x.Tecnico);
            _ = Query.OrderBy(x => x.DiaSemana)
                .ThenBy(x => x.Periodo)
                .ThenBy(x => x.Inicio);
        }
    }
}
