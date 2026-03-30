using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs
{
  public class TipoAparelhoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
  }
}
