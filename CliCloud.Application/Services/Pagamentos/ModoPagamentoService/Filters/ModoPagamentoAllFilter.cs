using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Filters;

public class ModoPagamentoAllFilter
{
    private List<TableFilter>? _filters;
    private List<TanstackColumnOrder>? _sorting;

    public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
    public List<TanstackColumnOrder> Sorting { get => _sorting ??= []; set => _sorting = value ?? []; }
    public string? FiltroBox { get; set; }
    public int? CodigoDe { get; set; }
    public int? CodigoAte { get; set; }
    public string? DescricaoDe { get; set; }
    public string? DescricaoAte { get; set; }
    public bool? FiltrarHistorico { get; set; }

    public string GetOrderByString()
    {
        if (Sorting.Count == 0) return "";
        var valid = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
        if (valid.Count == 0) return "";
        return string.Join(",", valid.Select(s => (s.Desc ? "-" : "") + s.Id));
    }
}
