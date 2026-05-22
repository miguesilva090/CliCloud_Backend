namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class GuardarPedidoConsultaMarcacaoResultDTO
{
  public Guid MarcacaoId { get; set; }
  public Guid? AdmissaoId { get; set; }
  public List<string> Avisos { get; set; } = [];
}
