using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService.Filters
{
    public class EstadoSinistroTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}