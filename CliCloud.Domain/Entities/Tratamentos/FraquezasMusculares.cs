#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    [Table("FraquezasMusculares", Schema = "Tratamentos")]
    public class FraquezasMusculares : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;
    }
}