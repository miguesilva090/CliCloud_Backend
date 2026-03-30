using Ardalis.Specification;
using CliCloud.Domain.Entities.Habilitacoes;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Specifications
{
    public class HabilitacaoMatchDescricao : Specification<Habilitacao>
    {
        public HabilitacaoMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
