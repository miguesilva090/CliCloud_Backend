using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Specifications;

public class NotificacaoTipoSearchTable : Specification<NotificacaoTipo>
{
  public NotificacaoTipoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
  {
    if (filters is { Count: > 0 })
    {
      foreach (TableFilter filter in filters)
      {
        switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
        {
          case "designacaotipo":
            if (!string.IsNullOrWhiteSpace(filter.Value))
              _ = Query.Where(x => x.DesignacaoTipo.Contains(filter.Value));
            break;
          default:
            break;
        }
      }
    }

    if (string.IsNullOrEmpty(dynamicOrder))
      _ = Query.OrderBy(x => x.DesignacaoTipo);
    else
      _ = Query.OrderBy(dynamicOrder);
  }
}
