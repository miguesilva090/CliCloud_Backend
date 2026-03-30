using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.GlicemiaCapilarService.Filters
{
    public class GlicemiaCapilarTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
