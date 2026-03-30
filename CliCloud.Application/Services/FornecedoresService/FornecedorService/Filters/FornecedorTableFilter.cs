using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.Filters
{
    public class FornecedorTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public FornecedorTableFilter()
        {
            Filters = [];
        }
    }
}
