#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Domain.Entities.Tratamentos
{
    [Table("TiposDeDor", Schema = "Tratamentos")]
    public class TipoDeDor : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

    }
}