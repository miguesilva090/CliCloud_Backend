using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class PatologiaLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
  }
}
