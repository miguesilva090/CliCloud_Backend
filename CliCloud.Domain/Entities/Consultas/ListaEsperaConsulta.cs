#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Consultas;

[Table("ListaEsperaConsulta", Schema = "Consultas")]
public class ListaEsperaConsulta : AuditableEntityWithSoftDelete
{
    public Guid UtenteId { get; set; }
    public Utente Utente { get; set; } = null!;
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    public Guid EspecialidadeId { get; set; }
    public Especialidade Especialidade { get; set; } = null!;
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    public Guid? PrioridadeId { get; set; }
    public Prioridade? Prioridade { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public TipoConsultaItem? TipoConsultaItem { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
    public Guid? ConsultaMarcacaoId { get; set; }
    public ConsultaMarcacao? ConsultaMarcacao { get; set; }
    public DateTime? ConvertidoEm { get; set; }
}