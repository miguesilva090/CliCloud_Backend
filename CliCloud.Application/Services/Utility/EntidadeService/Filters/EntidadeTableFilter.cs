using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.EntidadeService.Filters
{
    public class EntidadeTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EntidadeTableFilter()
        {
            Filters = [];
        }
    }
}
