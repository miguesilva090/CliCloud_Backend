using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Filters
{
    public class EstadoListaEsperaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
