using Ardalis.Specification;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;

public class UnidadeMedidaByClinicaSpec : Specification<UnidadeMedidaEntity>
{
    public UnidadeMedidaByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}
