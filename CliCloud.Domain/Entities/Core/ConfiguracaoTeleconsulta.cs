#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ConfiguracaoTeleconsulta", Schema = "Core")]
  public class ConfiguracaoTeleconsulta : AuditableEntity
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    public bool Ativo { get; set; }

    [StringLength(50)]
    public string Provider { get; set; } = "jitsi";

    [StringLength(300)]
    public string BaseMeetingUrl { get; set; } = "https://meet.jit.si";

    public bool JwtAtivo { get; set; }

    [StringLength(120)]
    public string? JwtAppId { get; set; }

    [StringLength(120)]
    public string? JwtApiKey { get; set; }

    [StringLength(120)]
    public string? JwtKid { get; set; }

    [StringLength(4000)]
    public string? JwtPrivateKey { get; set; }

    public int JanelaEntradaMinutosAntes { get; set; } = 15;
    public int DuracaoPadraoMinutos { get; set; } = 30;
    public bool PermitirEntradaAntesDoInicio { get; set; } = true;
    public bool LobbyAtivo { get; set; } = true;
  }
}
