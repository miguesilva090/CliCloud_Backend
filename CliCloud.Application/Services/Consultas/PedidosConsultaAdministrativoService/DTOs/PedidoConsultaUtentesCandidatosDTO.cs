namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class PedidoConsultaUtentesCandidatosDTO
{
  public bool ExisteNif { get; set; }
  public bool ExisteNome { get; set; }
  public bool ExisteEmail { get; set; }
  public bool ExisteTelemovel { get; set; }
  public List<PedidoConsultaUtenteCandidatoDTO> Utentes { get; set; } = [];
}
