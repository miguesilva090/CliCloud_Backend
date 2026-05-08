namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs
{
  public class DeleteMultipleMotivoConsultaRequest
  {
    public IEnumerable<Guid> Ids { get; set; } = [];
  }
}
