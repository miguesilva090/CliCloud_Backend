#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    [Table("ModeloAparelho", Schema = "Tratamentos")]
    public class ModeloAparelho : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Designacao { get; set; } = string.Empty;

        public Guid MarcaAparelhoId { get; set; }
        public MarcaAparelho MarcaAparelho { get; set; } = null!;
    }
}