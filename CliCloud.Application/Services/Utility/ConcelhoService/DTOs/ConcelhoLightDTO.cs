using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.ConcelhoService.DTOs
{
  public class ConcelhoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? DistritoId { get; set; }
    public string? DistritoNome { get; set; }
  }
}

