using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.Filters
{
    public class CentroSaudeTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public CentroSaudeTableFilter()
        {
            Filters = [];
        }
    }
}
