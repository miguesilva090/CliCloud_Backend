namespace CliCloud.Application.Services.Utility.TipoCartaService.DTOs
{
  public class CreateTipoCartaRequest
  {
    public string Descricao { get; set; } = string.Empty;
    public string? Obs { get; set; }
    public string? Caminho { get; set; }
  }
}
