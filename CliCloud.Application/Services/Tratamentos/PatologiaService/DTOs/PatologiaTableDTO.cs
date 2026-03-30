using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class PatologiaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public string? OrganismoNome { get; set; }
    public bool Inativo { get; set; }
  }
}
