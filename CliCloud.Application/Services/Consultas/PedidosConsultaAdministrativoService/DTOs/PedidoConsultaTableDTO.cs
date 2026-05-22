namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class PedidoConsultaTableDTO
{
  public int Codigo { get; set; }
  public int CodigoPedidosConsultaUtente { get; set; }
  public int CodigoEspecialidade { get; set; }
  public string? Nome { get; set; }
  public string? Telemovel { get; set; }
  public string? Email { get; set; }
  public string? Especialidade { get; set; }
  public DateTime Data { get; set; }
  public string Hora { get; set; } = string.Empty;
  public string? CodigoMedico { get; set; }
  public string? Medico { get; set; }
  public bool Agendado { get; set; }
  public bool EmailPedido { get; set; }
  public bool SmsPedido { get; set; }
  public bool EmailAgendado { get; set; }
  public bool SmsAgendado { get; set; }
  public bool Recusado { get; set; }
  public bool TemFicheiro { get; set; }
}
