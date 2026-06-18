#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Stocks;

[Table("Armazem", Schema = "Stocks")]
public class Armazem : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    public Guid ClinicaId { get; set; }

    public int Codigo { get; set; }

    [Required]
    [StringLength(40)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Morada { get; set; }

    [StringLength(50)]
    public string? Localidade { get; set; }

    public Guid? CodigoPostalId { get; set; }
    public CodigoPostal? CodigoPostal { get; set; }

    [StringLength(20)]
    public string? Telefone { get; set; }

    [StringLength(20)]
    public string? Fax { get; set; }

    public bool ArmazemGeral { get; set; }
}
