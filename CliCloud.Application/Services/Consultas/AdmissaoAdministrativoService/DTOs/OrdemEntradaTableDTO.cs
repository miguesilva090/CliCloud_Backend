using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class OrdemEntradaTableDTO : IDto
{
    public Guid Id { get; set; }
    public Guid? ConsultaMarcacaoId { get; set; }
    public DateTime? Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraChegada { get; set; }
    public int? Ordem { get; set; }
    public Guid UtenteId { get; set; }
    public string? UtenteNumero { get; set; }
    public string? UtenteNome { get; set; }
    public string? MedicoNome { get; set; }
    public string? EspecialidadeDesignacao { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
    public bool? Confirmado { get; set; }
    public StatusConsulta? StatusConsulta { get; set; }
    public string? StatusConsultaLabel { get; set; }
    public DateTime? DataHoraMarcacao { get; set; }
    public bool ConsultaPromovida { get; set; }
    public Guid? ConsultaId { get; set; }
    public Guid CreatedBy { get; set; }
    public string? CreatedByNome { get; set; }
}