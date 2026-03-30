#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Alergias
{
    [Table("AlergiasUtenteObs", Schema = "Alergias")]
    public class AlergiasUtenteObs : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid UtenteId { get; set; }

        [ForeignKey(nameof(UtenteId))]
        public Utente? Utente { get; set; }

        public string? Observacoes { get; set; }

        public string? InformacaoImportante { get; set; }
    }
}