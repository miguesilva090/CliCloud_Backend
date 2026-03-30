using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Specifications
{
    public class HorarioTecnicoSearchList : Specification<HorarioTecnico>
    {
        public HorarioTecnicoSearchList(string? keyword = "")
        {
            _ = Query.Include(x => x.Tecnico);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Tecnico != null && x.Tecnico.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
