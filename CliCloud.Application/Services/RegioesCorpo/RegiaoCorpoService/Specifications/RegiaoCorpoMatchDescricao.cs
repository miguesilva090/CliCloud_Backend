using Ardalis.Specification;
using CliCloud.Domain.Entities.RegioesCorpo;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Specifications
{
    public class RegiaoCorpoMatchDescricao : Specification<RegiaoCorpo>
    {
        public RegiaoCorpoMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }

            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
