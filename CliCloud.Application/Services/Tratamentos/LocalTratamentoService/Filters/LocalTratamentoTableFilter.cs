using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Filters
{
    public class LocalTratamentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
