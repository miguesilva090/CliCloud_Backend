#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas;

/// <summary>Pedido externo GlobalBooking (legado: dbo.PedidosConsulta; novo: Consultas.PedidoConsulta).</summary>
[Table("PedidoConsulta", Schema = "Consultas")]
public class PedidoConsulta : BaseEntity<int>
{
  [Column("Codigo")]
  public new int Id
  {
    get => base.Id;
    set => base.Id = value;
  }

  public int CodigoPedidosConsultaUtente { get; set; }
  public PedidoConsultaUtente? UtentePedido { get; set; }

  public int CodigoEspecialidade { get; set; }
  public DateTime Data { get; set; }

  [Required]
  [StringLength(5)]
  public string Hora { get; set; } = string.Empty;

  [StringLength(10)]
  public string? CodigoMedico { get; set; }

  /// <summary>Clínica do pedido (sessão / utilizador). Substitui o int legado <c>Filtro</c>.</summary>
  public Guid ClinicaId { get; set; }

  /// <summary>Legado ASPcli (empresa/filtro int). Não usar no projeto novo.</summary>
  [Obsolete("Usar ClinicaId (Guid). Mantido apenas por compatibilidade de coluna.")]
  public int? Filtro { get; set; }

  public bool Agendado { get; set; }
  public bool EmailPedido { get; set; }
  public bool SmsPedido { get; set; }
  public bool EmailAgendado { get; set; }
  public bool SmsAgendado { get; set; }
  public bool Recusado { get; set; }

  public int? CodigoAdmissao { get; set; }

  [StringLength(260)]
  public string? Ficheiro { get; set; }
}
