using Ardalis.Specification;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Specifications;

public class ArtigoByClinicaSpec : Specification<ArtigoEntity>
{
    public ArtigoByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}