using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;

public class GuardarPedidoConsultaMarcacaoRequest : IDto
{
  public Guid? UtenteId { get; set; }
  public bool ForcarNovoUtente { get; set; }
  public Guid MedicoId { get; set; }
  public Guid EspecialidadeId { get; set; }
  public Guid OrganismoId { get; set; }
  public Guid? TipoConsultaId { get; set; }
  public Guid? TipoAdmissaoId { get; set; }
  public Guid? MotivoConsultaId { get; set; }
  public DateTime Data { get; set; }
  public TimeSpan Hora { get; set; }
  public string? Obs { get; set; }
  public bool EnviarEmail { get; set; }
  public bool EnviarSms { get; set; }
}
