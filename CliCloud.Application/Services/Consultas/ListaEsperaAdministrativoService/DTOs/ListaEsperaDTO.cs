using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class ListaEsperaDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public string? UtenteNumero { get; set; }
    public string? UtenteNome { get; set; }
    public Guid? MedicoId { get; set; }
    public string? MedicoNome { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }
    public Guid? PrioridadeId { get; set; }
    public string? PrioridadeDesignacao { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
    public Guid? ConsultaMarcacaoId { get; set; }
    public DateTime? ConvertidoEm { get; set; }
}