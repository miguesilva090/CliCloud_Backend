using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.ConcelhoService.Filters
{
    public class ConcelhoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public ConcelhoTableFilter()
        {
          Filters = [];
        }
    }
}
