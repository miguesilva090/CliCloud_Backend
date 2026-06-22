using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Filters;

public class ArtigoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
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
    
}