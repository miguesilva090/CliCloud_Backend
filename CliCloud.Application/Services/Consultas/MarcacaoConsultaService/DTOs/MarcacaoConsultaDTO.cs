using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs
{
  /// <summary>
  /// DTO alinhado com a entidade Consultas.ConsultaMarcacao.
  /// Representa uma marcação de consulta (agenda), não o acto clínico.
  /// </summary>
  public class MarcacaoConsultaDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public Guid? ConsultaId { get; set; }
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? TecnicoId { get; set; }
    public Guid? FuncionarioId { get; set; }
    public Guid? MedicoExternoId { get; set; }
    public Guid? SalaId { get; set; }

    public DateTime? Data { get; set; }
    public TimeSpan? HoraMarcacao { get; set; }

    public Guid? MotivoConsultaId { get; set; }
    public Guid? TipoAdmissaoId { get; set; }

    public string? NumDestacavel { get; set; }
    public bool EmTratamento { get; set; }
    public int? StatusConsulta { get; set; }

    public string? Obs { get; set; }
  }
}

