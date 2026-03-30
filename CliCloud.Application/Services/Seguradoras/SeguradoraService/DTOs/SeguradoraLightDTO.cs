using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs
{
  public class SeguradoraLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Abreviatura { get; set; }
  }
}
