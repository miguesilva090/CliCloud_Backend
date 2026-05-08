namespace CliCloud.Application.Services.Consultas.SalaService.DTOs
{
  public class DeleteMultipleSalaRequest
  {
    public IEnumerable<Guid> Ids { get; set; } = [];
  }
}
