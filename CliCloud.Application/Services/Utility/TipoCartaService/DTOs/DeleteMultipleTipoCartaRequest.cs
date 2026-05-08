namespace CliCloud.Application.Services.Utility.TipoCartaService.DTOs
{
  public class DeleteMultipleTipoCartaRequest
  {
    public IEnumerable<Guid> Ids { get; set; } = [];
  }
}
