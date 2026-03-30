using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Filters
{
    public class FuncionarioTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public FuncionarioTableFilter()
        {
            Filters = [];
        }
    }
}
