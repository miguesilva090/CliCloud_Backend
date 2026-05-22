#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas;

/// <summary>Utente associado a pedido GlobalBooking (legado: dbo.PedidosConsultaUtente).</summary>
[Table("PedidoConsultaUtente", Schema = "Consultas")]
public class PedidoConsultaUtente : BaseEntity<int>
{
  [Column("Codigo")]
  public new int Id
  {
    get => base.Id;
    set => base.Id = value;
  }

  [Required]
  [StringLength(100)]
  public string Nome { get; set; } = string.Empty;

  public int Codinst { get; set; }

  [Required]
  [StringLength(250)]
  public string Email { get; set; } = string.Empty;

  [Required]
  [StringLength(20)]
  public string Telemovel { get; set; } = string.Empty;

  [StringLength(15)]
  public string? NIF { get; set; }
}
