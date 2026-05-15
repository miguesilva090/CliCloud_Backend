#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("TiposAdmissao", Schema = "Consultas")]
  public class TipoAdmissao : AuditableEntityWithSoftDelete
  {
    [Required]
    [StringLength(80)]
    public string Designacao { get; set; } = string.Empty;

    public int? CodigoLegado { get; set; }
  }
}