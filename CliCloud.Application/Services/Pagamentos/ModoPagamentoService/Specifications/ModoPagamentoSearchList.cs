using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;

public class ModoPagamentoSearchList : Specification<ModoPagamentoEntity>
{
    public ModoPagamentoSearchList(string? keyword, Guid clinicaId, bool apenasAtivos)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId)
            .Include(x => x.TipoPagamento)
            .Include(x => x.ContaBancaria);

        if (apenasAtivos)
            _ = Query.Where(x => !x.Historico);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(k) ||
                x.Abreviatura.Contains(k) ||
                x.Codigo.ToString().Contains(k));
        }

        _ = Query.OrderBy(x => x.Descricao);
    }
}
