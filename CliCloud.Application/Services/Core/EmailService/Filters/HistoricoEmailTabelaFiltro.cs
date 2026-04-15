using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Core.EmailService.Filters;

public class HistoricoEmailTabelaFiltro : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
}
