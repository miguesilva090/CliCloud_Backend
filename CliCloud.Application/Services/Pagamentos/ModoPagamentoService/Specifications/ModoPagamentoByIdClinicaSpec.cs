using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;

public class ModoPagamentoByIdClinicaSpec : Specification<ModoPagamentoEntity>
{
    public ModoPagamentoByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId)
            .Include(x => x.TipoPagamento)
            .Include(x => x.ContaBancaria);
    }
}
