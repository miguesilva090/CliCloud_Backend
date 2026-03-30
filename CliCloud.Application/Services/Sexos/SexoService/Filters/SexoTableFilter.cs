using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Sexos.SexoService.Filters
{
    public class SexoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public SexoTableFilter()
        {
            Filters = [];
        }
    }
}

