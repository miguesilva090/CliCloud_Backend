using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs
{
  public class ConsultaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? HoraFim { get; set; }
    public string? Sala { get; set; }
    public Guid? UtenteId { get; set; }
    public string? UtenteNome { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }
    public Guid? MedicoId { get; set; }
    public string? MedicoNome { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public Guid? TecnicoId { get; set; }
    public bool? Confirmado { get; set; }
    public bool? Efectuado { get; set; }
    public bool? Faltou { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
  }
}

