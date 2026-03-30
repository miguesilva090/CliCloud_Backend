using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Filters
{
    public class EmpresaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EmpresaTableFilter()
        {
            Filters = [];
        }
    }
}

