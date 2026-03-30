using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Filters
{
    public class CategoriaProcedimentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
