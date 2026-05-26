using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs;

public sealed class ConsultaDoDiaDTO : IDto
{
  public Guid Id { get; set; }
  public Guid? ConsultaId { get; set; }
  public Guid? ConsultaMarcacaoId { get; set; }
  public Guid? AdmissaoId { get; set; }
  public string Origem { get; set; } = string.Empty;

  public Guid? UtenteId { get; set; }
  public string? UtenteNumero { get; set; }
  public string? UtenteNome { get; set; }

  public Guid? MedicoId { get; set; }
  public string? MedicoNome { get; set; }
  public Guid? EspecialidadeId { get; set; }
  public string? EspecialidadeDesignacao { get; set; }
  public Guid? OrganismoId { get; set; }
  public string? OrganismoNome { get; set; }

  public DateTime? Data { get; set; }
  public string? DataLabel { get; set; }
  public string? HoraInicio { get; set; }
  public string? HoraFim { get; set; }
  public string? HoraChegada { get; set; }

  public Guid? TipoConsultaId { get; set; }
  public string? TipoConsultaDesignacao { get; set; }
  public Guid? TipoAdmissaoId { get; set; }
  public string? TipoAdmissaoDesignacao { get; set; }

  public string? Diagnostico { get; set; }
  public int? StatusConsulta { get; set; }
  public string? StatusConsultaLabel { get; set; }
  public bool? Confirmado { get; set; }
  public bool? Efetuado { get; set; }
  public bool? Faltou { get; set; }
}
