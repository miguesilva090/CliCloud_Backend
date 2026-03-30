using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Specifications
{
    public class FraquezasMuscularesMatchDescricao : Specification<FraquezasMusculares>
    {
        public FraquezasMuscularesMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
