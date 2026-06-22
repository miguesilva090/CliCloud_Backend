using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Stocks.ArtigoService.Filters;
using Microsoft.EntityFrameworkCore;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Specifications;

public class ArtigoSearchTable : Specification<ArtigoEntity>
{
    public ArtigoSearchTable(ArtigoTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId)
            .Include(x => x.Armazem);

        if (!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            string fb = filter.FiltroBox.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(fb) ||
                x.NumeroArtigo.Contains(fb) ||
                x.Codigo.ToString().Contains(fb));
        }

        if (filter.CodigoDe.HasValue)
            _ = Query.Where(x => x.Codigo >= filter.CodigoDe.Value);
        if (filter.CodigoAte.HasValue)
            _ = Query.Where(x => x.Codigo <= filter.CodigoAte.Value);
        if (!string.IsNullOrWhiteSpace(filter.NumeroArtigoDe))
            _ = Query.Where(x => string.Compare(x.NumeroArtigo, filter.NumeroArtigoDe.Trim()) >= 0);
        if (!string.IsNullOrWhiteSpace(filter.NumeroArtigoAte))
            _ = Query.Where(x => string.Compare(x.NumeroArtigo, filter.NumeroArtigoAte.Trim()) <= 0);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoDe))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoDe.Trim()) >= 0);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoAte))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoAte.Trim()) <= 0);
        if (filter.Inativo.HasValue)
            _ = Query.Where(x => x.Inativo == filter.Inativo.Value);
        if (filter.Descontinuado.HasValue)
            _ = Query.Where(x => x.Descontinuado == filter.Descontinuado.Value);
        if (filter.TipoArtigo.HasValue)
            _ = Query.Where(x => x.TipoArtigo == filter.TipoArtigo.Value);

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
                    case "numeroartigo":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.NumeroArtigo.Contains(f.Value));
                        break;
                    case "descricao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Descricao.Contains(f.Value));
                        break;
                    case "inativo":
                        if (bool.TryParse(f.Value, out bool inativo))
                            _ = Query.Where(x => x.Inativo == inativo);
                        break;
                    case "descontinuado":
                        if (bool.TryParse(f.Value, out bool descontinuado))
                            _ = Query.Where(x => x.Descontinuado == descontinuado);
                        break;
                    case "tipoartigo":
                        if (int.TryParse(f.Value, out int tipo) &&
                            Enum.IsDefined(typeof(Domain.Enums.TipoArtigoStocks), tipo))
                            _ = Query.Where(x => (int)x.TipoArtigo == tipo);
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.Codigo)
            : Query.OrderBy(dynamicOrder);
    }
}
