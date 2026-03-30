using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs
{
  public class TipoAparelhoDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
