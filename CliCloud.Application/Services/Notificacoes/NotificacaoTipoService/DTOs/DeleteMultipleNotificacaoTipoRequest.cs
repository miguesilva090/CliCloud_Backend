namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;

public class DeleteMultipleNotificacaoTipoRequest
{
  public IEnumerable<Guid> Ids { get; set; } = [];
}
