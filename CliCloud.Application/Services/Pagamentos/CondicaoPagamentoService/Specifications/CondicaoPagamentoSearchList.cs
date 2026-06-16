using Ardalis.Specification;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;

public class CondicaoPagamentoSearchList : Specification<CondicaoPagamentoEntity>
{
    public CondicaoPagamentoSearchList(string? keyword, Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x => x.Descricao.Contains(k) || x.Codigo.ToString().Contains(k));
        }

        _ = Query.OrderBy(x => x.Descricao);
    }
}
