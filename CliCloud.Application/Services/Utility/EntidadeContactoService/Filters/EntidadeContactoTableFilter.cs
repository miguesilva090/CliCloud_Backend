using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.Filters
{
    public class EntidadeContactoTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
