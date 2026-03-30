using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.Filters
{
    public class GoniometriasTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public GoniometriasTableFilter()
        {
            Filters = [];
        }
    }
}
