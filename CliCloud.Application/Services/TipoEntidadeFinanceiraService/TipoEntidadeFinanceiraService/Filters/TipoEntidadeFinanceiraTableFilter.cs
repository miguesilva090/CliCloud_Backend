using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Filters
{
    public class TipoEntidadeFinanceiraTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public TipoEntidadeFinanceiraTableFilter()
        {
            Filters = [];
        }
    }
}
