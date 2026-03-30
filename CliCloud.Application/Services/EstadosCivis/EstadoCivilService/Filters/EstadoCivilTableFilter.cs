using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Filters
{
    public class EstadoCivilTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EstadoCivilTableFilter()
        {
            Filters = [];
        }
    }
}
