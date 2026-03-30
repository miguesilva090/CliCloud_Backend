using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.ReciboService.DTOs
{
  public class ReciboLightDTO : IDto
  {
    public Guid Id { get; set; }
    public int NumeroDocumento { get; set; }
    public DateTime? Data { get; set; }
    public decimal? TotalLiquido { get; set; }
  }
}
