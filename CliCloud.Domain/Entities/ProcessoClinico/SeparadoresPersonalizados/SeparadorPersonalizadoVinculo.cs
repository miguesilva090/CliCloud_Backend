#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

public enum TipoVinculoSeparador
{
    Medico = 1,
    Especialidade = 2
}

[Table("SeparadorPersonalizadoVinculo", Schema = "ProcessoClinico")]
public class SeparadorPersonalizadoVinculo : AuditableEntity
{
    [Key]
    public new Guid Id { get; set; }

    [Required]
    public Guid SeparadorPersonalizadoId { get; set; }

    [ForeignKey(nameof(SeparadorPersonalizadoId))]
    public SeparadorPersonalizado SeparadorPersonalizado { get; set; } = null!;

    [Required]
    public TipoVinculoSeparador Tipo { get; set; }

    [Required]
    public Guid EntidadeId { get; set; }
}
