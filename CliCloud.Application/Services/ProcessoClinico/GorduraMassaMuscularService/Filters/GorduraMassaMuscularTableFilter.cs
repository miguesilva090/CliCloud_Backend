using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Filters
{
    public class GorduraMassaMuscularTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
