using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications
{
    public class HorarioMedicoSearchList : Specification<HorarioMedico>
    {
        public HorarioMedicoSearchList(string? keyword = "")
        {
            _ = Query.Include(x => x.Medico);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Medico != null && x.Medico.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
