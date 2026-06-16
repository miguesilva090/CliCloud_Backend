#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Domain.Entities.Pagamentos;

[Table("ModoPagamento", Schema = "Pagamentos")]
public class ModoPagamento: AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    public Guid ClinicaId { get; set; }

    public int Codigo { get; set; }

    [Required]
    [StringLength(50)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string Abreviatura { get; set; } = string.Empty;

    public TipoPagamento? TipoPagamento { get; set; }
    public bool TemNumAssociado { get; set; }
    public bool TemContaBancaria { get; set; }
    public Guid? ContaBancariaId { get; set; }
    public ContaBancaria? ContaBancaria { get; set; }
    public bool Historico { get; set; }
}