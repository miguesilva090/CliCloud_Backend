using Ardalis.Specification;
using CliCloud.Domain.Entities.Habilitacoes;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Specifications
{
    public class HabilitacaoSearchList : Specification<Habilitacao>
    {
        public HabilitacaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
