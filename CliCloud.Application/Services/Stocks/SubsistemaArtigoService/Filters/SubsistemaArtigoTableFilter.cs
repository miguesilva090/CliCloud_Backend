using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Filters;

public class SubsistemaArtigoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
    public string? FiltroBox { get; set; }
    public Guid? ArtigoId { get; set; }
    public Guid? OrganismoId { get; set; }
    public bool? Inativo { get; set; }
    
}