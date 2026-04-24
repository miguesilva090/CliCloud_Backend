#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    /// <summary>
    /// Local de tratamento (ex: Sala Verde, Sala Laranja, Sala Rosa).
    /// Utilizado em Tratamento.LocalTratamentoId.
    /// </summary>
    [Table("LocaisTratamento", Schema = "Tratamentos")]
    public class LocalTratamento : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Designacao { get; set; } = string.Empty;
    }
}
