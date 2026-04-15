#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

[Table("SeparadorVinculo", Schema = "ProcessoClinico")]
public class SeparadorVinculo : AuditableEntity
{
    [Key]
    public new Guid Id { get; set; }

    [Required]
    public Guid SeparadorId { get; set; }

    [ForeignKey(nameof(SeparadorId))]
    public Separador Separador { get; set; } = null!;

    [Required]
    public TipoVinculoSeparador Tipo { get; set; }

    [Required]
    public Guid EntidadeId { get; set; }
}
