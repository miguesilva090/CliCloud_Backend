using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Filters;

public class CredenciaisSnsTableFilter : PaginationFilter
{
    public string Modulo { get; set; } = CredenciaisSnsModulo.Especialidades;
    public List<TableFilter> Filters { get; set; } = [];
}
