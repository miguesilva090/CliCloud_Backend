using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.TipoCartaService.DTOs
{
  public class TipoCartaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? Obs { get; set; }
    public string? Caminho { get; set; }
  }
}
