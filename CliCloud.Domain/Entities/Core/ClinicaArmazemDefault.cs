using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ClinicaArmazemDefault", Schema = "Core")]
  public class ClinicaArmazemDefault : AuditableEntity
  {
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public bool ArmazemGeral { get; set; }

    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;
  }
}
