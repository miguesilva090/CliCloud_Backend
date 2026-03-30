using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService.Filters
{
    public class EntidadePessoaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EntidadePessoaTableFilter()
        {
            Filters = [];
        }
    }
}
