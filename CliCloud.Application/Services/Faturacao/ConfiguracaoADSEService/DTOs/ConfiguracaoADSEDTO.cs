using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.DTOs
{
  public class ConfiguracaoADSEDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string? UrlADSE { get; set; } = string.Empty;
    public string? Dominio { get; set; } = string.Empty;
    public string? Utilizador { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public Guid? OrganismoId { get; set; }
    public int? NumeroLocal { get; set; }
    public string? PasswordLocal { get; set; } = string.Empty;
    public string? UrlPasta { get; set; } = string.Empty;
  }
}
