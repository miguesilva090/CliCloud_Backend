using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoAdministrativoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? OrganismoId { get; set; }
    public DateTime? Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; } 
    public Guid? TipoConsultaId { get; set; }
    public Guid? TipoAdmissaoId { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
    public int? StatusConsulta { get; set; }
    public bool? Confirmado { get; set; }
    public bool? Efetuado { get; set; }

    public string? UtenteNome { get; set; }
    public string? UtenteNumero { get; set; }
    public string? MedicoNome { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
    public Guid? SalaId { get; set; }
    public string? SalaNome { get; set; }
    public Guid? AdmissaoId { get; set; }
}