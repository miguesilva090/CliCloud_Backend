#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Prescricao
{
    [Table("MedicacaoCronica", Schema = "Prescricao")]
    public class MedicacaoCronica : AuditableEntityWithSoftDelete
    {
        [Required] public Guid UtenteId { get; set; }
        [ForeignKey(nameof(UtenteId))] public Utente? Utente { get; set; }

        [Required, StringLength(50)] public string Cnpem { get; set; } = string.Empty;
        [StringLength(50)] public string? EmbId { get; set; }

        [Required, StringLength(500)] public string Designacao { get; set; } = string.Empty;
        [StringLength(254)] public string? Dosagem { get; set; }
        [StringLength(500)] public string? DescricaoEmbalagem { get; set; }
        [StringLength(200)] public string? FormaFarmaceutica { get; set; }
        [StringLength(500)] public string? PrincipioAtivo { get; set; }
        [StringLength(1000)] public string? Posologia { get; set; }

        public int TipoLinha { get; set; } = 1;

        public DateTime DataInicio { get; set; } 
        public DateTime? DataFim { get; set; }
    }
}