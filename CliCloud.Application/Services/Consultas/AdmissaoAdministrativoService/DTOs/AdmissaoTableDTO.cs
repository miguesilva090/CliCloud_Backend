using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AdmissaoTableDTO : IDto
{
  public Guid Id { get; set; }
  public DateTime? Data { get; set; }
  public TimeSpan? HoraInicio { get; set; }
  public Guid UtenteId { get; set; }
  public string? UtenteNumero { get; set; }
  public string? UtenteNome { get; set; }
  public string? MedicoNome { get; set; }
  public string? EspecialidadeDesignacao { get; set; }
  public string? OrganismoNome { get; set; }
  public string? SalaNome { get; set; }
  public string? Credencial { get; set; }
  public string? TipoAdmissaoDesignacao { get; set; }
  public StatusConsulta? StatusConsulta { get; set; }
  public bool? Confirmado { get; set; }
  public bool? Efetuado { get; set; }
  public int? Ordem { get; set; }
  public OrigemAdmissao Origem { get; set; }
}
