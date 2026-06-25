using Ardalis.Specification;
using ZonaComercialEntity = CliCloud.Domain.Entities.Faturacao.ZonaComercial;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.Specifications;

public class ZonaComercialByClinicaSpec : Specification<ZonaComercialEntity>
{
    public ZonaComercialByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}