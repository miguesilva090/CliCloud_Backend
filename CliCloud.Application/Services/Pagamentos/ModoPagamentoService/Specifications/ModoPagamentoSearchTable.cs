using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Filters;
using Microsoft.EntityFrameworkCore;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;

public class ModoPagamentoSearchTable : Specification<ModoPagamentoEntity>
{
    public ModoPagamentoSearchTable(ModoPagamentoTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId)
            .Include(x => x.TipoPagamento);

        if (!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            string fb = filter.FiltroBox.Trim();
            _ = Query.Where(x => x.Descricao.Contains(fb));
        }

        if (filter.CodigoDe.HasValue)
            _ = Query.Where(x => x.Codigo >= filter.CodigoDe.Value);
        if (filter.CodigoAte.HasValue)
            _ = Query.Where(x => x.Codigo <= filter.CodigoAte.Value);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoDe))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoDe.Trim()) >= 0);
        if (!string.IsNullOrWhiteSpace(filter.DescricaoAte))
            _ = Query.Where(x => string.Compare(x.Descricao, filter.DescricaoAte.Trim()) <= 0);

        if (filter.FiltrarHistorico.HasValue)
            _ = Query.Where(x => x.Historico == filter.FiltrarHistorico.Value);

        if (filter.Filters != null)
        {
            foreach (var f in filter.Filters)
            {
                switch ((f.Id ?? "").ToLowerInvariant())
                {
                    case "codigo":
                        if (int.TryParse(f.Value, out int codigo))
                            _ = Query.Where(x => x.Codigo == codigo);
                        break;
                    case "descricao":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Descricao.Contains(f.Value));
                        break;
                    case "abreviatura":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Abreviatura.Contains(f.Value));
                        break;
                    case "historico":
                        if (bool.TryParse(f.Value, out bool historico))
                            _ = Query.Where(x => x.Historico == historico);
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.Codigo)
            : Query.OrderBy(dynamicOrder);
    }
}
