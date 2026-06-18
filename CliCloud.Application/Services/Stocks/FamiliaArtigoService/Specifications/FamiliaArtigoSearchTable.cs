using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Filters;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoSearchTable : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoSearchTable(FamiliaArtigoTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (filter.ParentId.HasValue)
            _ = Query.Where(x => x.ParentId == filter.ParentId);
        else
            _ = Query.Where(x => x.ParentId == null);

        if (!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            var fb = filter.FiltroBox.Trim();
            _ = Query.Where(x => x.Descricao.Contains(fb) || x.Codigo.ToString().Contains(fb));
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
                        if (int.TryParse(f.Value, out var codigo))
                            _ = Query.Where(x => x.Codigo == codigo);
                        break;
                    case "descricao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Descricao.Contains(f.Value));
                        break;
                    case "nivel":
                        if (int.TryParse(f.Value, out var nivel))
                            _ = Query.Where(x => x.Nivel == nivel);
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.Codigo)
            : Query.OrderBy(dynamicOrder);
    }
}