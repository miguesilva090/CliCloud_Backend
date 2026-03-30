using Ardalis.Specification;
using CliCloud.Domain.Entities.Profissoes;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.Specifications
{
    public class ProfissaoMatchDescricao : Specification<Profissao>
    {
        public ProfissaoMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
