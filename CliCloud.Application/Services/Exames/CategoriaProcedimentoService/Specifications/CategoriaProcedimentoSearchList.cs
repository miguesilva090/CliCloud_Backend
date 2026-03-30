using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Specifications
{
    public class CategoriaProcedimentoSearchList : Specification<CategoriaProcedimento>
    {
        public CategoriaProcedimentoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao != null && x.Descricao.Contains(keyword));
            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
