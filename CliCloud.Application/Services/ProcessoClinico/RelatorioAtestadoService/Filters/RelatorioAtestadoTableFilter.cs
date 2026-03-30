using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Filters
{
    public class RelatorioAtestadoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
