using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;

public class NotificacaoTipoLightDTO : IDto
{
  public Guid Id { get; set; }
  public string DesignacaoTipo { get; set; } = string.Empty;
}
