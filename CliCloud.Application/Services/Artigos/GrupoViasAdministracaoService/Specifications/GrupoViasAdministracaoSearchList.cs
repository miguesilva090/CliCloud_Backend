using Ardalis.Specification;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Specifications
{
    public class GrupoViasAdministracaoSearchList : Specification<GrupoViasAdministracao>
    {
        public GrupoViasAdministracaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
