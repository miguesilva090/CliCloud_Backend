#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("MotivosConsulta", Schema = "Consultas")]
  public class MotivoConsulta : AuditableEntity
  {
    [Required]
    [StringLength(80)]
    public string Designacao { get; set; } = string.Empty;
  }
}
