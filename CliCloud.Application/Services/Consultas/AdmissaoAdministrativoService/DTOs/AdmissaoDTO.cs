using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AdmissaoDTO : IDto
{
  public Guid Id { get; set; }
  public Guid UtenteId { get; set; }
  public string? UtenteNumero { get; set; }
  public string? UtenteNome { get; set; }
  public Guid? ConsultaMarcacaoId { get; set; }
  public Guid? MedicoId { get; set; }
  public Guid? EspecialidadeId { get; set; }
  public Guid? TecnicoId { get; set; }
  public Guid? FuncionarioId { get; set; }
  public Guid? MedicoExternoId { get; set; }
  public Guid? SalaId { get; set; }
  public Guid? MotivoConsultaId { get; set; }
  public Guid? TipoAdmissaoId { get; set; }
  public Guid? TipoConsultaId { get; set; }
  public Guid? OrganismoId { get; set; }
  public Guid? SeguradoraId { get; set; }
  public Guid? TratamentoId { get; set; }
  public DateTime? Data { get; set; }
  public TimeSpan? HoraInicio { get; set; }
  public TimeSpan? HoraFim { get; set; }
  public TimeSpan? HoraChegada { get; set; }
  public StatusConsulta? StatusConsulta { get; set; }
  public OrigemAdmissao Origem { get; set; }
  public bool? Confirmado { get; set; }
  public bool? Efetuado { get; set; }
  public string? Credencial { get; set; }
  public int? CredencialExterna { get; set; }
  public string? NumDestacavel { get; set; }
  public int? Ordem { get; set; }
  public int? Sinistrado { get; set; }
  public int? Justificacao { get; set; }
  public string? MotivoJustificacao { get; set; }
  public string? Diagnostico { get; set; }
  public Guid? DoencaPrincipalId { get; set; }
  public string? DoencaPrincipalCodigo { get; set; }
  public string? DoencaPrincipalTitulo { get; set; }
  public Guid? DoencaSecundariaId { get; set; }
  public string? DoencaSecundariaCodigo { get; set; }
  public string? DoencaSecundariaTitulo { get; set; }
  public string? Obs { get; set; }
  public DateTime? DataHoraMarcacao { get; set; }
  public List<AdmissaoServicoDTO> Servicos { get; set; } = [];
}
