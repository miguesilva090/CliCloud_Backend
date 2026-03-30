using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Specifications
{
    public class HorarioTecnicoDiaSearchList : Specification<HorarioTecnicoDia>
    {
        public HorarioTecnicoDiaSearchList(string? keyword = "")
        {
            _ = Query.Include(x => x.HorarioTecnico)
                .ThenInclude(x => x.Tecnico);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.HorarioTecnico != null && x.HorarioTecnico.Tecnico != null && x.HorarioTecnico.Tecnico.Nome.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.DiaSemana)
                .ThenBy(x => x.Periodo)
                .ThenBy(x => x.Inicio); // default sort order
        }
    }
}
