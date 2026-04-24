#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    /// <summary>
    /// Prioridade (ex: Urgente, Normal, Primeira Prescrição).
    /// Chave primária é Guid herdado de AuditableEntity; sem IdPrioridade.
    /// </summary>
    [Table("Prioridades", Schema = "Tratamentos")]
    public class Prioridade : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; } = string.Empty;
    }
}
