using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.Specifications
{
    public class PrioridadeSearchList : Specification<Prioridade>
    {
        public PrioridadeSearchList(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
