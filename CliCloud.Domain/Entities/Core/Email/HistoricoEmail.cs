#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Core.Email;

[Table("HistoricoEmail", Schema = "Core")]
public class HistoricoEmail : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    [StringLength(250)] 
    public string? AssuntoEmail { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string CorpoEmail { get; set; } = string.Empty;

    [StringLength(250)]
    public string? EmailDestino { get; set; }

    [StringLength(250)]
    public string? NomeUtente { get; set; }

    [StringLength(30)]
    public string? Contacto { get; set; }

    public DateTime DataHoraCriacao { get; set; }
    public DateTime? DataHoraEnvio { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pendente";

    [Column(TypeName = "nvarchar(max)")]
    public string? MensagemErro { get; set; }

    [StringLength(20)] 
    public string Modulo { get; set; } = string.Empty;
}