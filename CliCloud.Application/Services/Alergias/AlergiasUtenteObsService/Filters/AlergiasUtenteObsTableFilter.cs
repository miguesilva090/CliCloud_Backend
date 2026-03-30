using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.Filters
{
    public class AlergiasUtenteObsTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
