using Ardalis.Specification;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.Specifications
{
    public class EspecialidadeSearchList : Specification<Especialidade>
    {
        public EspecialidadeSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.Include(x => x.CategoriaEspecialidade);
            _ = Query.OrderBy(x => x.Nome); // default sort order
        }
    }
}
