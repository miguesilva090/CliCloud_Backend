using Ardalis.Specification;
using CliCloud.Domain.Entities.Stocks;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Specifications;

public class SubsistemaArtigoByIdClinicaSpec : Specification<SubsistemaArtigo>
{
    public SubsistemaArtigoByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query
            .Where(x => x.Id == id && x.ClinicaId == clinicaId)
            .Include(x => x.Artigo)
            .Include(x => x.Organismo);
    }
}