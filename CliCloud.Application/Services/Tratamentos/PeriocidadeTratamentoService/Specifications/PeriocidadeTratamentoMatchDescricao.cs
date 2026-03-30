using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Specifications
{
    public class PeriocidadeTratamentoMatchDescricao : Specification<PeriocidadeTratamento>
    {
        public PeriocidadeTratamentoMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
