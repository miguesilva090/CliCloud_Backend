using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class DeleteMultiplePatologiaRequest : IDto
  {
    public IEnumerable<Guid> Ids { get; set; } = [];
  }
}
