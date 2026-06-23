using Ardalis.Specification;
using CliCloud.Domain.Entities.Stocks;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Specifications;

public class SubsistemaArtigoMatchCompositeSpec : Specification<SubsistemaArtigo>
{
    public SubsistemaArtigoMatchCompositeSpec(Guid clinicaId, Guid artigoId, Guid organismoId)
    {
         _ = Query.Where(x => 
            x.ClinicaId == clinicaId &&
            x.ArtigoId == artigoId && 
            x.OrganismoId == organismoId);
    }
}