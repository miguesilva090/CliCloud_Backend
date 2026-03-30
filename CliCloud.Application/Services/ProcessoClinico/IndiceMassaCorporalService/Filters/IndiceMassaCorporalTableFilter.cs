using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.IndiceMassaCorporalService.Filters
{
    public class IndiceMassaCorporalTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
