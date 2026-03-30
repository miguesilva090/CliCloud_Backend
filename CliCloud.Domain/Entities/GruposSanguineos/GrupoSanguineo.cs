#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.GruposSanguineos
{
    [Table("GrupoSanguineo", Schema = "Utility")]
    public class GrupoSanguineo : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; } = string.Empty;
    }
}
