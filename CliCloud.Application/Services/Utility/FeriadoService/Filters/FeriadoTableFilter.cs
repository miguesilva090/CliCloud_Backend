using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.FeriadoService.Filters;

public class FeriadoTableFilter : PaginationFilter
{
    public DateTime? DataDe { get; set; }
    public DateTime? DataAte { get; set; }
    public string? Designacao { get; set; }
    public bool? Ativo { get; set; }
}