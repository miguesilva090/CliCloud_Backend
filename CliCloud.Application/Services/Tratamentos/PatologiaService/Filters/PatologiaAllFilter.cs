using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.Filters
{
  public class PatologiaAllFilter
  {
    private List<TableFilter>? _filters;
    public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
    public static string? GetOrderByString() => "Designacao";
  }
}
