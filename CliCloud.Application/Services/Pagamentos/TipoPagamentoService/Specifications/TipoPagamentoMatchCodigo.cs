using Ardalis.Specification;
using TipoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.TipoPagamento;

namespace CliCloud.Application.Services.Pagamentos.TipoPagamentoService.Specifications;

public class TipoPagamentoMatchCodigo : Specification<TipoPagamentoEntity>
{
    public TipoPagamentoMatchCodigo(string codigo)
    {
        string c = codigo.Trim().ToUpperInvariant();
        _ = Query.Where(x => x.Codigo == c);
    }
}
