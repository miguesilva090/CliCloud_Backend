using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Specifications
{
    public class HorarioMedicoDiaSearchList : Specification<HorarioMedicoDia>
    {
        public HorarioMedicoDiaSearchList(string? keyword = "")
        {
            _ = Query.Include(x => x.HorarioMedico)
                .ThenInclude(x => x.Medico);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.HorarioMedico != null && x.HorarioMedico.Medico != null && x.HorarioMedico.Medico.Nome.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.DiaSemana)
                .ThenBy(x => x.Periodo)
                .ThenBy(x => x.Inicio); // default sort order
        }
    }
}
