#nullable disable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Especialidades;


namespace CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica
{
    [Table("HistoriasClinicas", Schema = "HistoriaClinica")]
    public class HistoriaClinica : AuditableEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid MedicoId { get; set; }
        public Medico Medico { get; set; } = null!;

        public DateTime Data { get; set; }

        public string? Hora { get; set; }

        public Guid? EspecialidadeId { get; set; }
        public Especialidade? Especialidade { get; set; }

        public string Obs { get; set; } = string.Empty;

    }
}