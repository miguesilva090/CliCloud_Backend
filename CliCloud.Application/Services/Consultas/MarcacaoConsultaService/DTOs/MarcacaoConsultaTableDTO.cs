using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs
{
  public class MarcacaoConsultaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid? ConsultaId { get; set; }
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public DateTime? Data { get; set; }
    public TimeSpan? HoraMarcacao { get; set; }
    public Guid? MotivoConsultaId { get; set; }
    public Guid? TipoAdmissaoId { get; set; }
    public int? StatusConsulta { get; set; }

    // Dados de apresentação (não usados para joins)
    public string? UtenteNumero { get; set; }
    public string? UtenteNome { get; set; }
    public string? OrganismoCodigo { get; set; }
    public string? OrganismoNome { get; set; }

    /// <summary>Data formatada para exibição (yyyy-MM-dd). Preenchido no backend.</summary>
    public string? DataLabel { get; set; }
    /// <summary>Hora formatada para exibição (HH:mm). Preenchido no backend.</summary>
    public string? HoraMarcacaoLabel { get; set; }
    /// <summary>Rótulo do estado (Display do enum StatusConsulta). Preenchido no backend.</summary>
    public string? StatusConsultaLabel { get; set; }

    public DateTime CreatedOn { get; set; }
  }
}

