using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TemperaturaCorporalService.Filters
{
    public class TemperaturaCorporalTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
