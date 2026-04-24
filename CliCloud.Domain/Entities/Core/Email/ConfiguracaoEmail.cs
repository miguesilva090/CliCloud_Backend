# nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Core.Email;

[Table("ConfiguracaoEmail", Schema = "Core")]
public class ConfiguracaoEmail : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;
    [StringLength(200)] public string Username { get; set; } = string.Empty;
    [StringLength(500)] public string Server { get; set; } = string.Empty;
    [StringLength(500)] public string Password { get; set; } = string.Empty;
    public int Porta { get; set; }
    public bool UseSSL { get; set; }
    public int TipoServico { get; set; }
    [StringLength(150)] public string? Inbox { get; set; }
    [StringLength(150)] public string? Outbox { get; set; }
    [StringLength(250)] public string? Email { get; set; }
    [StringLength(300)] public string? DisplayName { get; set; }
    public bool PermitirEliminarEmail { get; set; }
}