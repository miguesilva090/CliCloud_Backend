using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs
{
  public class TeleconsultaJoinDTO : IDto
  {
    public Guid SessaoId { get; set; }
    public string MeetingId { get; set; } = string.Empty;
    public string MeetingUrl { get; set; } = string.Empty;
    public string Papel { get; set; } = "Profissional";
    public bool Moderador { get; set; }
    public DateTime ExpiraEmUtc { get; set; }
  }
}
