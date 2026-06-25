#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;


namespace CliCloud.Domain.Entities.Faturacao;

[Table("ZonaComercial", Schema = "Faturacao")]
public class ZonaComercial : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    public int Codigo { get; set; }

    [Required]
    [StringLength(40)]
    public string Descricao { get; set; } = string.Empty;

    public int? CodigoInternoLegado { get; set; }
}