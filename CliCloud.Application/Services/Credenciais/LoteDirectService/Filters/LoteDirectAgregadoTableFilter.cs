using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Filters;

public class LoteDirectAgregadoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
}
