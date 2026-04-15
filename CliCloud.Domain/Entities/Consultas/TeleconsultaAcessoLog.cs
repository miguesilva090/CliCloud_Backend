#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("TeleconsultaAcessoLog", Schema = "Consultas")]
  public class TeleconsultaAcessoLog : AuditableEntity
  {
    public Guid ClinicaId { get; set; }
    public Guid TeleconsultaSessaoId { get; set; }
    public Guid ConsultaMarcacaoId { get; set; }

    [StringLength(120)]
    public string? UserId { get; set; }

    [StringLength(30)]
    public string Papel { get; set; } = "Sistema";

    [StringLength(40)]
    public string Acao { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Mensagem { get; set; }

    public bool Sucesso { get; set; }

    [StringLength(100)]
    public string? Ip { get; set; }

    [StringLength(500)]
    public string? UserAgent { get; set; }

    public TeleconsultaSessao TeleconsultaSessao { get; set; } = null!;
  }
}
