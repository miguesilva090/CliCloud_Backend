#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Core.Email;

[Table("ConfiguracaoEmailAutomatica", Schema = "Core")]
public class ConfiguracaoEmailAutomatica : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;
    [StringLength(20)] public string Codigo { get; set; } = string.Empty;
    [StringLength(200)] public string Descricao { get; set; } = string.Empty;
    public int Ativo { get; set; }
    public int Diasantecedencia { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string Textomensagem { get; set; } = string.Empty;
}