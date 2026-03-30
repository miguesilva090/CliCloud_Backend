using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Filters
{
  public class TipoAparelhoAllFilter
  {
    private List<TableFilter>? _filters;
    private List<TanstackColumnOrder>? _sorting;

    public List<TableFilter> Filters
    {
      get => _filters ??= new List<TableFilter>();
      set => _filters = value ?? new List<TableFilter>();
    }

    public List<TanstackColumnOrder> Sorting
    {
      get => _sorting ??= new List<TanstackColumnOrder>();
      set => _sorting = value ?? new List<TanstackColumnOrder>();
    }

    public TipoAparelhoAllFilter()
    {
      Filters = new List<TableFilter>();
      Sorting = new List<TanstackColumnOrder>();
    }

    public string GetOrderByString()
    {
      if (Sorting.Count == 0) return "";
      var valid = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
      if (valid.Count == 0) return "";
      return string.Join(",", valid.Select(s => s.Desc ? "-" + s.Id : s.Id));
    }
  }
}
