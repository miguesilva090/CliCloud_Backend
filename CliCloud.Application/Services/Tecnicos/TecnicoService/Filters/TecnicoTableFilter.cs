using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService.Filters
{
    public class TecnicoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public TecnicoTableFilter()
        {
            Filters = [];
        }
    }
}
