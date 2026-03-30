#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Alergias
{
    [Table("GrauAlergia", Schema = "Alergias")]
    public class GrauAlergia : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; } = string.Empty;
    }
}
