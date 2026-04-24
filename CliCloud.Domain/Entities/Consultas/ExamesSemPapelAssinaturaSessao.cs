#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Domain.Entities.Consultas
{
  [Index(nameof(UtilizadorId), IsUnique = true)]
  [Table("ExamesSemPapelAssinaturaSessao", Schema = "Consultas")]
  public class ExamesSemPapelAssinaturaSessao : AuditableEntityWithSoftDelete
  {
    public Guid UtilizadorId { get; set; }

    [StringLength(50)]
    public string CMedico { get; set; } = string.Empty;

    [StringLength(20)]
    public string TipoCartao { get; set; } = string.Empty;

    [StringLength(4000)]
    public string DigestValue { get; set; } = string.Empty;

    [StringLength(4000)]
    public string SignatureValue { get; set; } = string.Empty;

    [StringLength(8000)]
    public string Assinatura { get; set; } = string.Empty;

    [StringLength(8000)]
    public string AssinaturaSubCA { get; set; } = string.Empty;

    public DateTime AtualizadoEmUtc { get; set; } = DateTime.UtcNow;
  }
}
