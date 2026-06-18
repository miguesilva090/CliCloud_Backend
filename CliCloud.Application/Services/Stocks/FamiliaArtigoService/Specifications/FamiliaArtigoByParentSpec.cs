using Ardalis.Specification;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoByParentSpec : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoByParentSpec(Guid? parentId, Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (parentId.HasValue)
            _ = Query.Where(x => x.ParentId == parentId);
        else
            _ = Query.Where(x => x.ParentId == null);
    }
}