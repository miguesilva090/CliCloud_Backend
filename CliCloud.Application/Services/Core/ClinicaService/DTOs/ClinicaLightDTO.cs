using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class ClinicaLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Abreviatura { get; set; }
  }
}
