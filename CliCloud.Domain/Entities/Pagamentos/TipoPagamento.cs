#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Pagamentos;

[Table("TipoPagamento", Schema = "Pagamentos")]
public class TipoPagamento: AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    [Required]
    [StringLength(3)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Descricao { get; set; } = string.Empty;
}