#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames
{
    [Table("RelatorioExames", Schema = "ProcessoClinico")]
    public class RelatorioExames : AuditableEntity
    {

        [Key]
        public new Guid Id { get; set; }

        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid MedicoId { get; set; }
        public Medico Medico { get; set; } = null!;

        public string? Texto { get; set; }

    }
}