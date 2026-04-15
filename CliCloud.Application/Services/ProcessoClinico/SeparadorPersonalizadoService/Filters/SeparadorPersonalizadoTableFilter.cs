using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Filters;

public class SeparadorPersonalizadoTableFilter : PaginationFilter
{
    public string? Keyword { get; set; }
}
