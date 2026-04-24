namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;

/// <summary>Modo de listagem para caixa de notificações.</summary>
public enum NotificacaoListMode
{
  /// <summary>Recebidas pelo utilizador autenticado.</summary>
  Inbox = 0,

  /// <summary>Enviadas pelo utilizador autenticado.</summary>
  Enviadas = 1,

  /// <summary>Atualizações da clínica (sem destinatário individual).</summary>
  AtualizacoesClinica = 2,
}
