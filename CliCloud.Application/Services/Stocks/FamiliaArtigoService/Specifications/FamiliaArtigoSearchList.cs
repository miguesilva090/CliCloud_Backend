using Ardalis.Specification;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoSearchList : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoSearchList(string? keyword , Guid clinicaId)
    {
        Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var k = keyword.Trim();
            Query.Where(x => x.Descricao.Contains(k) || x.Codigo.ToString().Contains(k));
        }

        Query.OrderBy(x => x.Codigo);
    }
}