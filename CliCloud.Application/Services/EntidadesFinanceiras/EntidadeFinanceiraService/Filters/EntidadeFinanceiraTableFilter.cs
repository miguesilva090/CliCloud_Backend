using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Filters
{
    public class EntidadeFinanceiraTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EntidadeFinanceiraTableFilter()
        {
            Filters = [];
        }
    }
}
