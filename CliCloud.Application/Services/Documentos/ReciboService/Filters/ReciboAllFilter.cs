using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.ReciboService.Filters
{
  public class ReciboAllFilter
  {
    private List<TableFilter>? _filters;
    private List<TanstackColumnOrder>? _sorting;

    public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
    public List<TanstackColumnOrder> Sorting { get => _sorting ??= []; set => _sorting = value ?? []; }

    public ReciboAllFilter() { Filters = []; Sorting = []; }

    public string GetOrderByString()
    {
      if (Sorting.Count == 0) return "";
      var v = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
      if (v.Count == 0) return "";
      return string.Join(",", v.Select(s => s.Desc ? "-" + s.Id : s.Id));
    }
  }
}
