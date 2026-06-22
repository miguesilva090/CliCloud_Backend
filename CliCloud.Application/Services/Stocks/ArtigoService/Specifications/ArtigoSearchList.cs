using Ardalis.Specification;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Specifications;

public class ArtigoSearchList : Specification<ArtigoEntity>
{
    public ArtigoSearchList(string? keyword, Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(k) ||
                x.NumeroArtigo.Contains(k) ||
                x.Codigo.ToString().Contains(k));
        }

        _ = Query.OrderBy(x => x.Codigo);
    }
}
