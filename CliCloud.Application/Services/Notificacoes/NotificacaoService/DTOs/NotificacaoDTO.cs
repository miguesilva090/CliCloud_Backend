using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;

public class NotificacaoDTO : IDto
{
  public Guid Id { get; set; }
  public string Titulo { get; set; } = string.Empty;
  public string? Descricao { get; set; }
  public int Estado { get; set; }
  /// <summary>Texto equivalente a EstadoDescr no legado.</summary>
  public string? EstadoDesignacao { get; set; }
  public int Prioridade { get; set; }
  /// <summary>Texto equivalente a PrioridadeDescr no legado.</summary>
  public string? PrioridadeDesignacao { get; set; }
  public Guid NotificacaoTipoId { get; set; }
  public string? TipoDesignacao { get; set; }
  public Guid RemetenteId { get; set; }
  public Guid? DestinatarioUtilizadorId { get; set; }
  public Guid? ClinicaDestinoId { get; set; }
  /// <summary>«Anúncio à clínica» vs «Destinatário único», alinhado ao modo legado tipoDestinatario.</summary>
  public string AlcanceResumo { get; set; } = string.Empty;
  public DateTime? DataLeitura { get; set; }
  public Guid? LeituraPor { get; set; }
  public DateTime CreatedOn { get; set; }
  public DateTime? LastModifiedOn { get; set; }
}
