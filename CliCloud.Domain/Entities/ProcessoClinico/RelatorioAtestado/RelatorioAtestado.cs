using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado
{
    [Table("RelatorioAtestado", Schema = "ProcessoClinico")]
    public class RelatorioAtestado : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid MedicoId { get; set; }
        public Medico Medico { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string TextoHtml { get; set; } = null!;

        public DateTime? AssinadoEm { get; set; }
    }
}