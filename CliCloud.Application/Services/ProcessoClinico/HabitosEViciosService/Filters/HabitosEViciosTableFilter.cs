using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Filters
{
    public class HabitosEViciosTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
