#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("LicencaUserClinicaMap", Schema = "Core")]
  public class LicencaUserClinicaMap : AuditableEntityWithSoftDelete
  {
    public Guid ClienteIdLicencas { get; set; }
    public Guid UserIdLicencas { get; set; }
    public Guid ClinicaId { get; set; }
    public Clinica? Clinica { get; set; }
    public bool IsDefault { get; set; }
    public bool Ativo { get; set; }
  }
}