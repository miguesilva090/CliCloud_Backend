using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Filters
{
    public class TipoConsultaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
