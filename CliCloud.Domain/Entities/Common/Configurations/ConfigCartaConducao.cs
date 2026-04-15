using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Common.Configurations;

[Table("ConfigCartaConducao", Schema = "Core")]
public class ConfigCartaConducao : AuditableEntity
{
    [Required]
    public Guid ClinicaId { get; set; }

    public string? UrlOnline { get; set; }
    public string? UrlOffline { get; set; }

    public string? Utilizador { get; set; }
    public string? Password { get; set; }

    public int AutoridadeSaudePublica { get; set; }
}