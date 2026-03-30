using Ardalis.Specification;
using CliCloud.Domain.Entities.Artigos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Specifications
{
    public class GrupoViasAdministracaoByIdWithVias : Specification<GrupoViasAdministracao>
    {
        public GrupoViasAdministracaoByIdWithVias()
        {
            _ = Query
                .Include(g => g.Vias)
                .ThenInclude(l => l.Via);
        }
    }
}
