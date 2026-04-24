using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Domain.Entities.HistoriaClinica;

[Table("HistoriasClinicas", Schema = "HistoriaClinica")]
public class HistoriaClinica : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    public Guid UtenteId { get; set; }
    public Utente Utente { get; set; } = null!;

    public Guid MedicoId { get; set; }
    public Medico Medico { get; set; } = null!;

    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }

    public DateTime Data { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string ObsHtml { get; set; } = string.Empty;

    public bool Inativo { get; set; }
}