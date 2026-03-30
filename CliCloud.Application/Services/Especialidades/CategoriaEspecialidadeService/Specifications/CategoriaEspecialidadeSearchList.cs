using Ardalis.Specification;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Specifications
{
    public class CategoriaEspecialidadeSearchList : Specification<CategoriaEspecialidade>
    {
        public CategoriaEspecialidadeSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao); // default sort order
        }
    }
}
