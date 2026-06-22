using Ardalis.Specification;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;

public class UnidadeMedidaSearchList : Specification<UnidadeMedidaEntity>
{
    public UnidadeMedidaSearchList(string? keyword, Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(k) ||
                x.Codigo.ToString().Contains(k));
        }

        _ = Query.OrderBy(x => x.Codigo);
    }
}
