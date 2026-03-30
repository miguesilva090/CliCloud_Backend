using Ardalis.Specification;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Specifications
{
    public class GrupoViasAdministracaoMatchName : Specification<GrupoViasAdministracao>
    {
        public GrupoViasAdministracaoMatchName(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }

            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
