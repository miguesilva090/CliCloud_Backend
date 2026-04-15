using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs
{
  public class TeleconsultaSessaoDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public Guid ConsultaMarcacaoId { get; set; }
    public string MeetingId { get; set; } = string.Empty;
    public string MeetingUrl { get; set; } = string.Empty;
    public string Provider { get; set; } = "jitsi";
    public string Status { get; set; } = "Criada";
    public DateTime InicioPrevistoUtc { get; set; }
    public DateTime FimPrevistoUtc { get; set; }
    public DateTime? InicioEfetivoUtc { get; set; }
    public DateTime? FimEfetivoUtc { get; set; }
    public bool LinksAtivos { get; set; }
    public DateTime? LinksRevogadosEmUtc { get; set; }
    public bool Ativo { get; set; }
  }
}
