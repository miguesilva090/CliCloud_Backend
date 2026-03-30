using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications
{
    public class EvolucaoTratamentoMatchDescricao : Specification<EvolucaoTratamento>
    {
        public EvolucaoTratamentoMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.ObservacaoClinica == descricao);
            }
            _ = Query.OrderBy(h => h.ObservacaoClinica);
        }
    }
}
