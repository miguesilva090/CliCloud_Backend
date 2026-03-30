using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Specifications
{
    public class LocalTratamentoSearchList : Specification<LocalTratamento>
    {
        public LocalTratamentoSearchList(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.Trim().ToLowerInvariant();
                _ = Query.Where(x =>
                    x.Designacao != null && x.Designacao.Contains(k, StringComparison.OrdinalIgnoreCase));
            }
            _ = Query.OrderBy(x => x.Designacao);
        }
    }
}
