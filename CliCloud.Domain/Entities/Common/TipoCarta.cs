#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliCloud.Domain.Entities.Common
{
  [Table("TiposCarta", Schema = "Comum")]
  public class TipoCarta : AuditableEntityWithSoftDelete
  {
    [Key]
    public new Guid Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Descricao { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Obs { get; set; }

    [StringLength(500)]
    public string? Caminho { get; set; }
  }
}
