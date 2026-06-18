using Ardalis.Specification;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;


namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;

public class FamiliaArtigoByClinicaSpec : Specification<FamiliaArtigoEntity>
{
    public FamiliaArtigoByClinicaSpec(Guid clinicaId)
    {
        Query.Where(x => x.ClinicaId == clinicaId);
    }
}