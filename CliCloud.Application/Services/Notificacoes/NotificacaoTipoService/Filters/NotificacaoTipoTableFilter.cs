using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Filters;

public class NotificacaoTipoTableFilter : PaginationFilter
{
  public List<TableFilter> Filters { get; set; } = [];
}
