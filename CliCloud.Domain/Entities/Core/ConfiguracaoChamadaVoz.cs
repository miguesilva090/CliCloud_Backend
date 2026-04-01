#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ConfiguracaoChamadaVoz", Schema = "Core")]
  public class ConfiguracaoChamadaVoz : AuditableEntity
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    public bool Ativo { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(10)]
    public string? Language { get; set; } = "pt";

    [StringLength(30)]
    public string? Tld { get; set; } = "pt";
  }
}
