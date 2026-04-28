using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;

public class NotificacaoTableDTO : IDto
{
  public Guid Id { get; set; }
  public string Titulo { get; set; } = string.Empty;
  public int Estado { get; set; }
  public string? EstadoDesignacao { get; set; }
  public int Prioridade { get; set; }
  public string? PrioridadeDesignacao { get; set; }
  public Guid NotificacaoTipoId { get; set; }
  public string? TipoDesignacao { get; set; }
  public Guid RemetenteId { get; set; }
  public Guid? DestinatarioUtilizadorId { get; set; }
  public Guid? ClinicaDestinoId { get; set; }
  public DateTime? DataLeitura { get; set; }
  public bool Lida { get; set; }
  public DateTime CreatedOn { get; set; }
}
