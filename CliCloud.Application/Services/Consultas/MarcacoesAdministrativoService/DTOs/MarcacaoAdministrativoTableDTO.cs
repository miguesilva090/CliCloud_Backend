using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoAdministrativoTableDTO : IDto 
{
    public Guid Id { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInicio { get; set; }
    public string? HoraFim { get; set; }
    public string? UtenteNumero { get; set; }
    public string? UtenteNome { get; set; }
    public string? MedicoNome { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public string? OrganismoNome { get; set; }
    public int? StatusConsulta { get; set; }
    public string? StatusConsultaLabel { get; set; }
}