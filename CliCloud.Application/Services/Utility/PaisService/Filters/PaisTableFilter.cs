using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.PaisService.Filters
{
    public class PaisTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } 

        public PaisTableFilter()
        {
            Filters = [];
        }
    }
}
