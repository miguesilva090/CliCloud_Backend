#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Notificacoes;

[Table("Notificacao", Schema = "Notificacoes")]
public class Notificacao : AuditableEntityWithSoftDelete
{
  [Key]
  public new Guid Id { get; set; }

  [Required]
  [StringLength(500)]
  public string Titulo { get; set; } = string.Empty;

  public string? Descricao { get; set; }

  public int Estado { get; set; }

  public int Prioridade { get; set; }

  public Guid NotificacaoTipoId { get; set; }

  [ForeignKey(nameof(NotificacaoTipoId))]
  public NotificacaoTipo? NotificacaoTipo { get; set; }

  public Guid RemetenteId { get; set; }

  public Guid? DestinatarioUtilizadorId { get; set; }

  public Guid? ClinicaDestinoId { get; set; }

  public DateTime? DataLeitura { get; set; }

  public Guid? LeituraPor { get; set; }
}
