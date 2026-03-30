#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    [Table("MotivosDesmarcacao", Schema = "Tratamentos")]
    public class MotivosDesmarcacao : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;
    }
}