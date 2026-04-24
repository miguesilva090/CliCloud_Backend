#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

[Table("SeparadorPersonalizado", Schema = "ProcessoClinico")]
public class SeparadorPersonalizado : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    [Required]
    public Guid ClinicaId { get; set; }

    [Required]
    public Guid UtilizadorId { get; set; }

    [Required]
    [MaxLength(150)]
    public string NomeSeparador { get; set; } = string.Empty;

    [Required]
    public Guid FormularioId { get; set; }

    [ForeignKey(nameof(FormularioId))]
    public FichaClinicaSecaoTemplate Formulario { get; set; } = null!;

    public int Ordem { get; set; }

    public bool Ativo { get; set; } = true;
}
