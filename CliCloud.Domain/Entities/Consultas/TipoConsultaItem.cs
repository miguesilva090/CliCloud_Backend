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

        /// <summary>Código do tipo no legado (TIPOS_CONSULTA); ex.: 1 = 1ª consulta.</summary>
        public int? CodigoLegado { get; set; }
    }
}
