#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.TaxasIva
{
    [Table("MotivoRetencao", Schema = "Utility")]
    public class MotivoRetencao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        public int Codigo { get; set; }

        [Required]
        [StringLength(150)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [StringLength(3)]
        public string TipoImposto { get; set; } = string.Empty;
    }
}
