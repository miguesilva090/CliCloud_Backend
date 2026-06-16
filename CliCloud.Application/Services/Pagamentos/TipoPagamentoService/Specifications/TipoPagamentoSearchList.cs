using Ardalis.Specification;
using TipoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.TipoPagamento;

namespace CliCloud.Application.Services.Pagamentos.TipoPagamentoService.Specifications;

public class TipoPagamentoSearchList : Specification<TipoPagamentoEntity>
{
    public TipoPagamentoSearchList(string? keyword = "")
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x => x.Codigo.Contains(k) || x.Descricao.Contains(k));
        }

        _ = Query.OrderBy(x => x.Descricao);
    }
}
