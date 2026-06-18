using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.Filters;

public class FamiliaArtigoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
    public string? FiltroBox { get; set; }
    public int? CodigoDe { get; set; }
    public int? CodigoAte { get; set; }
    public string? DescricaoDe { get; set; }
    public string? DescricaoAte { get; set; }
    public Guid? ParentId { get; set; }
}