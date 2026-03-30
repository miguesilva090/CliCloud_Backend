using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Filters
{
    public class PeriocidadeTratamentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public PeriocidadeTratamentoTableFilter()
        {
            Filters = [];
        }
    }
}
