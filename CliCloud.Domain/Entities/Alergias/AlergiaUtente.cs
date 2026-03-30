#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Alergias
{
    [Table("AlergiaUtente", Schema = "Alergias")]
    public class AlergiaUtente : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid UtenteId { get; set; }

        [ForeignKey(nameof(UtenteId))]
        public Utente? Utente { get; set; }

        public Guid? AlergiaId { get; set; }

        [ForeignKey(nameof(AlergiaId))]
        public Alergia? Alergia { get; set; }

        public Guid? GrauAlergiaId { get; set; }

        [ForeignKey(nameof(GrauAlergiaId))]
        public GrauAlergia? GrauAlergia { get; set; }

        public DateOnly? DataDesde { get; set; }

        public DateOnly? DataAte { get; set; }

        public string? Observacoes { get; set; }
    }
}