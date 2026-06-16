#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Pagamentos;

[Table("CondicaoPagamento", Schema = "Pagamentos")]
public class CondicaoPagamento: AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public int Codigo { get; set; }

    [Required]
    [StringLength(30)]
    public string Descricao { get; set; } = string.Empty;

    public int? NDiasPagamento { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Desconto { get; set; }
}