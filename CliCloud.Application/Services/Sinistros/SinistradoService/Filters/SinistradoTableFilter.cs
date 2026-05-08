using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Filters
{
    public class SinistradoTableFilter  : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}