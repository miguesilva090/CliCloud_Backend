using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class SetPedidoConsultaRecusadoRequest : IDto
{
  public bool Recusado { get; set; }
}
