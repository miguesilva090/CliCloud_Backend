#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("TipoAparelho", Schema = "Tratamentos")]
  public class TipoAparelho : AuditableEntity
  {

    [Key]
    public new Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Designacao { get; set; } = string.Empty;

    public ICollection<Aparelho> Aparelhos { get; set; } = new List<Aparelho>();
  }
}
