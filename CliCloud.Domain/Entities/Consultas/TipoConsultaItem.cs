#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas
{
    /// <summary>
    /// Tipo de consulta (ex: 1ª Consulta, AV. Final, Feriado, Teleconsulta).
    /// </summary>
    [Table("TiposConsulta", Schema = "Consultas")]
    public class TipoConsultaItem : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Designacao { get; set; } = string.Empty;
    }
}
