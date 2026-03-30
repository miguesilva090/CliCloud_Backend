using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs
{
  public class TipoAparelhoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Designacao { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}
