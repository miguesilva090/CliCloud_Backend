#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.RegioesCorpo
{
    [Table("RegiaoCorpo", Schema = "RegioesCorpo")]
    public class RegiaoCorpo : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;
    }
}