using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs
{
  public class SeguradoraTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Apolice { get; set; }
    public decimal? Avenca { get; set; }
    public string? Abreviatura { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}
