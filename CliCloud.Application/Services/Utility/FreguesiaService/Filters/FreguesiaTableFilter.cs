using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.FreguesiaService.Filters
{
    public class FreguesiaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public FreguesiaTableFilter()
        {
            Filters = [];
        }
    }
}
