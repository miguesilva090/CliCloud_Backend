using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.Filters
{
    public class RelatorioExamesTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
