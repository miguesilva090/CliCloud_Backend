#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Notificacoes;

[Table("NotificacaoTipo", Schema = "Notificacoes")]
public class NotificacaoTipo : AuditableEntityWithSoftDelete
{
  [Key]
  public new Guid Id { get; set; }

  [Required]
  [StringLength(60)]
  public string DesignacaoTipo { get; set; } = string.Empty;

  public bool ReservadoSistema { get; set; }
}
