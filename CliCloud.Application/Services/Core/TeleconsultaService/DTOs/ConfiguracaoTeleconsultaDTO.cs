using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.TeleconsultaService.DTOs
{
  public class ConfiguracaoTeleconsultaDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public bool Ativo { get; set; }
    public string Provider { get; set; } = "jitsi";
    public string BaseMeetingUrl { get; set; } = "https://meet.jit.si";
    public bool JwtAtivo { get; set; }
    public string? JwtAppId { get; set; }
    public string? JwtApiKey { get; set; }
    public string? JwtKid { get; set; }
    public string? JwtPrivateKey { get; set; }
    public int JanelaEntradaMinutosAntes { get; set; }
    public int DuracaoPadraoMinutos { get; set; }
    public bool PermitirEntradaAntesDoInicio { get; set; }
    public bool LobbyAtivo { get; set; }
  }
}
