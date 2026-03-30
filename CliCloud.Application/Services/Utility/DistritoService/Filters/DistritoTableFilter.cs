using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.DistritoService.Filters
{
    public class DistritoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public DistritoTableFilter()
        {
          Filters = [];
        }
    }
}
