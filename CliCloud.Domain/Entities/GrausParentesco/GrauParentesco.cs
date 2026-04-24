#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.GrausParentesco
{
    [Table("GrauParentesco", Schema = "Utility")]
    public class GrauParentesco : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; } = string.Empty;
    }
}
