using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Filters
{
    public class AntecedentesCirurgicosTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public AntecedentesCirurgicosTableFilter()
        {
            Filters = [];
        }
    }
}
