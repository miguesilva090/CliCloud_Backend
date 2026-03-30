using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Filters
{
    public class EstadoListaEsperaAllFilter
    {
        private List<TableFilter>? _filters;
        public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
        public static string? GetOrderByString() => "IdEstado";
    }
}
