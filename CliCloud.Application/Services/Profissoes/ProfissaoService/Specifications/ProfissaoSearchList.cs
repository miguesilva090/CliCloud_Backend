using Ardalis.Specification;
using CliCloud.Domain.Entities.Profissoes;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.Specifications
{
    public class ProfissaoSearchList : Specification<Profissao>
    {
        public ProfissaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
