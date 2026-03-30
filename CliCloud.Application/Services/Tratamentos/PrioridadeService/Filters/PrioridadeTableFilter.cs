using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.Filters
{
    public class PrioridadeTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
