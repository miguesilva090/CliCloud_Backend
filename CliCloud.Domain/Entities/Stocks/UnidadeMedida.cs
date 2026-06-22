#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Stocks;

[Table("UnidadeMedida", Schema = "Stocks")]
public class UnidadeMedida : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    public Guid ClinicaId { get; set; }

    /// <summary>Código de negócio (legado: CodigoUnidade).</summary>
    public int Codigo { get; set; }

    [Required]
    [StringLength(15)]
    public string Descricao { get; set; } = string.Empty;
}
