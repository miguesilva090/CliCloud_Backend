using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class ListaEsperaTableDTO : IDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public string? UtenteNumero { get; set; }
    public string? UtenteNome { get; set; }
    public string? UtenteTelefone { get; set; }
    public string? MedicoNome { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public string? OrganismoNome { get; set; }
    public string? PrioridadeDesignacao { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
    public DateTime Data { get; set; }
    public string? HoraInicio { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
    public bool Convertido { get; set; }
    public Guid? ConsultaMarcacaoId { get; set; }

}