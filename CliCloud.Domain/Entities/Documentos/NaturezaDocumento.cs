#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Documentos
{
    [Table("NaturezaDocumento", Schema = "Documentos")]
    public class NaturezaDocumento : AuditableEntityWithSoftDelete
    {
        [Required]
        [StringLength(1)]
        public string Sigla { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; } = string.Empty;
    }
}
