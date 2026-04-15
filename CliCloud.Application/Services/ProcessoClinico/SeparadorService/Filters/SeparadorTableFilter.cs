using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.Filters;

public class SeparadorTableFilter : PaginationFilter
{
    public string? Keyword { get; set; }
}
