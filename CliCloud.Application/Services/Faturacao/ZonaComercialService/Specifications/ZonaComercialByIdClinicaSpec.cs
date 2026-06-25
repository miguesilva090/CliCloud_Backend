using Ardalis.Specification;
using ZonaComercialEntity = CliCloud.Domain.Entities.Faturacao.ZonaComercial;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.Specifications;

public class ZonaComercialByIdClinicaSpec : Specification<ZonaComercialEntity>
{
    public ZonaComercialByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
    }
}