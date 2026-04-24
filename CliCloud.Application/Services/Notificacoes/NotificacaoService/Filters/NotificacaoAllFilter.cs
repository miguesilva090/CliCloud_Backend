using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;

public class NotificacaoAllFilter
{
  private List<TableFilter>? _filters;
  private List<TanstackColumnOrder>? _sorting;

  /// <summary>0=Inbox, 1=Enviadas, 2=Atualizações da clínica.</summary>
  public NotificacaoListMode ListMode { get; set; } = NotificacaoListMode.Inbox;

  public List<TableFilter> Filters
  {
    get => _filters ??= [];
    set => _filters = value ?? [];
  }

  public List<TanstackColumnOrder> Sorting
  {
    get => _sorting ??= [];
    set => _sorting = value ?? [];
  }

  public string GetOrderByString()
  {
    if (Sorting.Count == 0)
      return "";

    List<TanstackColumnOrder> validSortColumns = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
    if (validSortColumns.Count == 0)
      return "";

    string sortingString = "";
    int count = 1;
    foreach (TanstackColumnOrder sortColumn in validSortColumns)
    {
      sortingString += sortColumn.Desc ? "-" + sortColumn.Id : sortColumn.Id;
      if (count != validSortColumns.Count)
        sortingString += ",";
      count++;
    }

    return sortingString;
  }
}
