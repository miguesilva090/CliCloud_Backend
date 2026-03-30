using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs
{
  public class AparelhoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TipoAparelhoId { get; set; }
    public string? CodigoSerie { get; set; }
    public string? Local { get; set; }
    public bool Ocupado { get; set; }
  }
}
