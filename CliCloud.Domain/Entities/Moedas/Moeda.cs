#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Moedas
{
    [Table("Moeda", Schema = "Utility")]
    public class Moeda : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Plural { get; set; }

        [Required]
        public decimal Cambio { get; set; } = 1m;

        [StringLength(10)]
        public string? Abreviatura { get; set; }

        [StringLength(10)]
        public string? Centesimos { get; set; }

        [StringLength(50)]
        public string? CentesimoPlural { get; set; }

        [StringLength(3)]
        public string? Simbolo { get; set; }
    }
}
