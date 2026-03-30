using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs
{
  public class ConsultaLightDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? HoraFim { get; set; }
    public string? Sala { get; set; }
  }
}

