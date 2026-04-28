using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("HistoriaDentaria", Schema = "Estomatologia")]
    public class HistoriaDentaria : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        [Required]
        public Guid MedicoId { get; set; }
        public Medico Medico { get; set; } = null!;

        public DateTime DataRegisto { get; set; }

        [Required]
        public string HistoriaHtml { get; set; } = string.Empty;
    }
}
