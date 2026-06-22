using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Filters;

public class ArtigoAllFilter 
{
    private List<TableFilter>? _filters;
    private List<TanstackColumnOrder>? _sorting;

    public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
    public List<TanstackColumnOrder> Sorting { get => _sorting ??= []; set => _sorting = value ?? []; }
    public string? FiltroBox { get; set; }
    public int? CodigoDe { get; set; }
    public int? CodigoAte { get; set; }
    public string? NumeroArtigoDe { get; set; }
    public string? NumeroArtigoAte { get; set; }
    public string? DescricaoDe { get; set; }
    public string? DescricaoAte { get; set; }
    public bool? Inativo { get; set; }
    public bool? Descontinuado { get; set; }
    public TipoArtigoStocks? TipoArtigo { get; set; }


    public string GetOrderByString()
    {
        if(Sorting.Count == 0) return "";
        var valid = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
        if (valid.Count == 0) return "";
        return string.Join(",", valid.Select(s => (s.Desc ? "-" : "") + s.Id));
    }

}