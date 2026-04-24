using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;

public class NotificacaoTableFilter : PaginationFilter
{
  /// <summary>0=Inbox, 1=Enviadas, 2=Atualizações da clínica.</summary>
  public NotificacaoListMode ListMode { get; set; } = NotificacaoListMode.Inbox;

  public List<TableFilter> Filters { get; set; } = [];
}
