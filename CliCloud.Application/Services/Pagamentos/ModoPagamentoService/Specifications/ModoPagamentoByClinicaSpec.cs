using Ardalis.Specification;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;

public class ModoPagamentoByClinicaSpec : Specification<ModoPagamentoEntity>
{
    public ModoPagamentoByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}
