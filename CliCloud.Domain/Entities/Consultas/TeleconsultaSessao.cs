#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("TeleconsultaSessao", Schema = "Consultas")]
  public class TeleconsultaSessao : AuditableEntityWithSoftDelete
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    public Guid ConsultaMarcacaoId { get; set; }
    public ConsultaMarcacao ConsultaMarcacao { get; set; } = null!;

    [StringLength(120)]
    public string MeetingId { get; set; } = null!;

    [StringLength(500)]
    public string MeetingUrl { get; set; } = null!;

    [StringLength(50)]
    public string Provider { get; set; } = "jitsi";

    [StringLength(20)]
    public string Status { get; set; } = "Criada";

    public DateTime InicioPrevistoUtc { get; set; }
    public DateTime FimPrevistoUtc { get; set; }
    public DateTime? InicioEfetivoUtc { get; set; }
    public DateTime? FimEfetivoUtc { get; set; }

    [StringLength(1024)]
    public string? TokenMedico { get; set; }

    [StringLength(1024)]
    public string? TokenUtente { get; set; }

    public bool LinksAtivos { get; set; } = true;
    public DateTime? LinksRevogadosEmUtc { get; set; }

    public bool Ativo { get; set; } = true;
  }
}
