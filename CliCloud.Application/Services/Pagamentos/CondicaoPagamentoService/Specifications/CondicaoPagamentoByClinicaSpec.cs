using Ardalis.Specification;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;

public class CondicaoPagamentoByClinicaSpec : Specification<CondicaoPagamentoEntity>
{
    public CondicaoPagamentoByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}
