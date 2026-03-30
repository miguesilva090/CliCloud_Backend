#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Alergias
{
    [Table("Alergia", Schema = "Alergias")]
    public class Alergia : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Descricao { get; set; }
    }
}
