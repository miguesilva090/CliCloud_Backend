using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.GrauAlergiaService.Filters
{
    public class GrauAlergiaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
