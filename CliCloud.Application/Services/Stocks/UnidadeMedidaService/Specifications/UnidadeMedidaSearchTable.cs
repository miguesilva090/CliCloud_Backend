using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Filters;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;

public class UnidadeMedidaSearchTable : Specification<UnidadeMedidaEntity>
{
    public UnidadeMedidaSearchTable(UnidadeMedidaTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            string fb = filter.FiltroBox.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(fb) ||
                x.Codigo.ToString().Contains(fb));
        }

        if (filter.CodigoDe.HasValue)
            _ = Query.Where(x => x.Codigo >= filter.CodigoDe.Value);
        if (filter.CodigoAte.HasValue)
            _ = Query.Where(x => x.Codigo <= filter.CodigoAte.Value);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoDe))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoDe.Trim()) >= 0);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoAte))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoAte.Trim()) <= 0);

        if (filter.Filters != null)
        {
            foreach (var f in filter.Filters)
            {
                switch ((f.Id ?? "").ToLowerInvariant())
                {
                    case "codigo":
                        if (int.TryParse(f.Value, out int codigo))
                            _ = Query.Where(x => x.Codigo == codigo);
                        break;
                    case "descricao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Descricao.Contains(f.Value));
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.Codigo)
            : Query.OrderBy(dynamicOrder);
    }
}
