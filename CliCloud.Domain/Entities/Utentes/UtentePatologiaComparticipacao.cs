#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Utentes
{
    [Table("UtentePatologiaComparticipacao", Schema = "Utentes")]
    public class UtentePatologiaComparticipacao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid UtenteId { get; set; }

        [ForeignKey(nameof(UtenteId))]
        public Utente? Utente { get; set; }

        [Required]
        public int CodigoComparticipacao { get; set; }
        
        [MaxLength(200)]
        public string? Designacao { get; set; }
    }
}