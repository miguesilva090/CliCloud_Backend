using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Filters;

public class PedidoConsultaTableFilter : PaginationFilter
{
  public List<TableFilter> Filters { get; set; } = [];

  /// <summary>Clínica da sessão; se omitido, o serviço resolve via <see cref="ICurrentClinicaService"/>.</summary>
  public Guid? ClinicaId { get; set; }

  public bool? AgendadoSim { get; set; }
  public bool? AgendadoNao { get; set; }
  public bool? RecusadoSim { get; set; }
  public bool? RecusadoNao { get; set; }
  public bool? EmailPedidoSim { get; set; }
  public bool? EmailPedidoNao { get; set; }
  public bool? SmsPedidoSim { get; set; }
  public bool? SmsPedidoNao { get; set; }
  public bool? EmailAgendadoSim { get; set; }
  public bool? EmailAgendadoNao { get; set; }
  public bool? SmsAgendadoSim { get; set; }
  public bool? SmsAgendadoNao { get; set; }
  public DateTime? DataDe { get; set; }
  public DateTime? DataAte { get; set; }
  public string? CodigoMedicoDe { get; set; }
  public string? CodigoMedicoAte { get; set; }
}
