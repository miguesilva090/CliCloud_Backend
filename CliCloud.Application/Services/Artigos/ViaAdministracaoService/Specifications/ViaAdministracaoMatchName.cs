using Ardalis.Specification;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.Specifications
{
    public class ViaAdministracaoMatchName : Specification<ViaAdministracao>
    {
        public ViaAdministracaoMatchName(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }

            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
