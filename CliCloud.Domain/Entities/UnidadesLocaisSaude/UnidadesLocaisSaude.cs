#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.UnidadesLocaisSaude
{
  [Table("UnidadesLocaisSaude", Schema = "UnidadesLocaisSaude")]
  public class UnidadesLocaisSaude : AuditableEntity
  {
    [Required]
    public int Codigo { get; set; }

    [Required]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;

    // NIF (ou contribuinte) da ULS, se existir no dataset.
    [StringLength(50)]
    public string? Nif { get; set; }
  }
}

