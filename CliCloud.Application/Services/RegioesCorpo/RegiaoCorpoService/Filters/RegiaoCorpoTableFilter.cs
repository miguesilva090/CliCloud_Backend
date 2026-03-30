using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Filters
{
    public class RegiaoCorpoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public RegiaoCorpoTableFilter()
        {
            Filters = [];
        }
    }
}
