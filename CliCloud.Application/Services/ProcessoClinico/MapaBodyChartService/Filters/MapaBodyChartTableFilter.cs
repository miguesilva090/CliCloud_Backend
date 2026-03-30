using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Filters
{
    public class MapaBodyChartTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
