#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Sinistros
{
    [Table("EstadoSinistroItem", Schema = "Sinistros")]
    public class EstadoSinistroItem : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Designacao { get; set; } = string.Empty;
    }
}