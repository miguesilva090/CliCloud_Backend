using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.Filters
{
    public class PrioridadeAllFilter
    {
        private List<TableFilter>? _filters;
        public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
        public static string? GetOrderByString() => "Descricao";
    }
}
