using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;

public class OrdemEntradaRegistoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public string? UtenteNome { get; set; }
    public string? UtenteNumero { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }
    public Guid? MedicoId { get; set; }
    public string? MedicoNome { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? SalaId { get; set; }
    public string? SalaNome { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public Guid? TipoAdmissaoId { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public DateTime? Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public string? Obs { get; set; }
    public string? CreatedByNome { get; set; }
    public DateTime? DataHoraMarcacao { get; set; }

}