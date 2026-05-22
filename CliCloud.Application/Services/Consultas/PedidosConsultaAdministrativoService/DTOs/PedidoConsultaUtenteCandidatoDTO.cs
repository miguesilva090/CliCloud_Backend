namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class PedidoConsultaUtenteCandidatoDTO
{
  public Guid Id { get; set; }
  public string? Nome { get; set; }
  public string? NumeroUtente { get; set; }
  public string? NumeroContribuinte { get; set; }
  public string? Email { get; set; }
}
