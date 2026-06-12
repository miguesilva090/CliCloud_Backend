using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Filters
{
    public class ContaBancariaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}