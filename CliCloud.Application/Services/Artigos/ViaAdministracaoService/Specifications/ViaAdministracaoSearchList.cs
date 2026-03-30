using Ardalis.Specification;
using CliCloud.Domain.Entities.Artigos;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.Specifications
{
    public class ViaAdministracaoSearchList : Specification<ViaAdministracao>
    {
        public ViaAdministracaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
