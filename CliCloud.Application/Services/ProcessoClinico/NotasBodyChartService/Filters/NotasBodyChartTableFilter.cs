using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Filters
{
    public class NotasBodyChartTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
