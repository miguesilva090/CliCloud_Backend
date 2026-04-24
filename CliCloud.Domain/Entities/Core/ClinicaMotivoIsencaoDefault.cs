using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ClinicaMotivoIsencaoDefault", Schema = "Core")]
  public class ClinicaMotivoIsencaoDefault : AuditableEntityWithSoftDelete
  {
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    [StringLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [StringLength(200)]
    public string Descricao { get; set; } = string.Empty;
  }
}
