#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Domain.Entities.Consultas
{
  [Index(nameof(ClinicaId), nameof(RequisicaoId), IsUnique = true)]
  [Table("ExamesSemPapelOperacao", Schema = "Consultas")]
  public class ExamesSemPapelOperacao : AuditableEntity
  {
    public Guid ClinicaId { get; set; }

    [StringLength(120)]
    public string RequisicaoId { get; set; } = string.Empty;

    [StringLength(20)]
    public string? AreaPrestacao { get; set; }

    public bool Assinado { get; set; }
    public bool Comunicado { get; set; }
    public bool Lotes { get; set; }
    public bool IsencaoTaxa { get; set; }
    public bool ComTaxa { get; set; }
    public bool Pnp { get; set; }

    public DateTime UltimaOperacaoUtc { get; set; } = DateTime.UtcNow;
  }
}
