#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ClinicaAPIKey", Schema = "Core")]
  public class ClinicaApiKey : AuditableEntityWithSoftDelete
  {
    public Guid ClinicaId { get; set; }
    public Clinica? Clinica { get; set; }

    [Required]
    [StringLength(512)]
    public string ApiKey { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;
  }
}

