using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;

public class HistoricoConsultaAdministrativoRowDTO : IDto
{
  public Guid Id { get; set; }
  public Guid? UtenteId { get; set; }
  public DateTime? Data { get; set; }
  public string? HoraInic { get; set; }
  public string? UtenteNumero { get; set; }
  public string? UtenteNome { get; set; }
  public string? MedicoNome { get; set; }
  public string? OrganismoNome { get; set; }
  public string? EspecialidadeDesignacao { get; set; }
  public int? StatusConsulta { get; set; }
  public string? StatusConsultaLabel { get; set; }
  public bool? Pago { get; set; }
  public bool? Faturado { get; set; }
}
