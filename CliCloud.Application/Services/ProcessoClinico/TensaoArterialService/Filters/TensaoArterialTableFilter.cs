using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Filters
{
    public class TensaoArterialTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
