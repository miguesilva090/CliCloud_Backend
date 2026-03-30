using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Core.SmsService.Filters
{
    public class HistoricoSmsTabelaFiltro : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }
    }
}