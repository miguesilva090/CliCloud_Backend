using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.DTOs
{
  public class UnidadesLocaisSaudeLightDTO : IDto
  {
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nif { get; set; }
  }
}

