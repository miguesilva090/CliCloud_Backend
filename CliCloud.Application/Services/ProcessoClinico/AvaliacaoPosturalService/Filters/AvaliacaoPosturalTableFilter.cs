using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Filters
{
    public class AvaliacaoPosturalTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
