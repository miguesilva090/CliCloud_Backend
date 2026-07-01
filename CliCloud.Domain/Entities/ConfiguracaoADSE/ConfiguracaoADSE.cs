using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ConfiguracaoADSE;

[Table("ConfiguracaoADSE", Schema = "Faturacao")]
public class ConfiguracaoADSE : AuditableEntityWithSoftDelete
{
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
