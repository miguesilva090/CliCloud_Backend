using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;

public class NotificacaoTipoTableDTO : IDto
{
  public Guid Id { get; set; }
  public string DesignacaoTipo { get; set; } = string.Empty;
  public bool ReservadoSistema { get; set; }
  public DateTime CreatedOn { get; set; }
}
