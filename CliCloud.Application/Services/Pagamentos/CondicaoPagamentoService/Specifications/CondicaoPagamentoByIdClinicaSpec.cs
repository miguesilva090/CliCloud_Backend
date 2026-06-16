using Ardalis.Specification;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;

public class CondicaoPagamentoByIdClinicaSpec : Specification<CondicaoPagamentoEntity>
{
    public CondicaoPagamentoByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
    }
}
