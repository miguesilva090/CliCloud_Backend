using Ardalis.Specification;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;

public class UnidadeMedidaByIdClinicaSpec : Specification<UnidadeMedidaEntity>
{
    public UnidadeMedidaByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
    }
}
