#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("TiposAdmissao", Schema = "Consultas")]
  public class TipoAdmissao : AuditableEntity
  {
    [Required]
    [StringLength(80)]
    public string Designacao { get; set; } = string.Empty;
  }
}