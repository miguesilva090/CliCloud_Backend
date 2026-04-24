using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ClinicaTipoConsultaDefault", Schema = "Core")]
  public class ClinicaTipoConsultaDefault : AuditableEntityWithSoftDelete
  {
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    [StringLength(80)]
    public string Designacao { get; set; } = string.Empty;
  }
}
