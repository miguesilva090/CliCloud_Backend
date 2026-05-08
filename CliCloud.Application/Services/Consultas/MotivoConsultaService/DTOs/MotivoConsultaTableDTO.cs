using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs
{
  public class MotivoConsultaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
  }
}
