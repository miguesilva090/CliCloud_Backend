using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class RequisicaoEspByNumeroSpec : Specification<RequisicaoEsp>
{
    public RequisicaoEspByNumeroSpec(string numeroRequisicao, bool incluirFilhos = false)
    {
        Query.Where(x => x.NumeroRequisicao == numeroRequisicao.Trim());

        if (incluirFilhos)
        {
            Query
                .Include(x => x.Linhas)
                .Include(x => x.EfetuadosNaoPrescritos);
        }
    }
}
