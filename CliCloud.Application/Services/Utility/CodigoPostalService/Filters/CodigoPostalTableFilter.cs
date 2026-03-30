using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.Filters
{
    public class CodigoPostalTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public CodigoPostalTableFilter()
        {
            Filters = [];
        }
    }
}
