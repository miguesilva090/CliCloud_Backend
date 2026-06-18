using Ardalis.Specification;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoByIdClinicaSpec : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
    }
}