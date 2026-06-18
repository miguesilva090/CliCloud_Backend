using Ardalis.Specification;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoChildrenSpec : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoChildrenSpec(Guid parentId)
    {
        _ = Query.Where(x => x.ParentId == parentId);
    }
}
