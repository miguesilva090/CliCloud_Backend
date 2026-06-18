using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Stocks.ArmazemService.Filters;

public class ArmazemTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
    public string? FiltroBox { get; set; }
    public int? CodigoDe { get; set; }
    public int? CodigoAte { get; set; }
    public string? NomeDe { get; set; }
    public string? NomeAte { get; set; }
    public bool? ArmazemGeral { get; set; }
}
