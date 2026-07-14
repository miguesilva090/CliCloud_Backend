using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Fornecedores.FornecedorService.Filters
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
