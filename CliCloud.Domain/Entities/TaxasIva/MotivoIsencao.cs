#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.TaxasIva
{
    /// <summary>
    /// Motivo de isenção de IVA. Obrigatório quando a TaxaIva selecionada for "Isento".
    /// </summary>
    [Table("MotivoIsencao", Schema = "Utility")]
    public class MotivoIsencao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>Código SAFT-PT do motivo de isenção (legado: CodigoSAFT).</summary>
        [StringLength(12)]
        public string? CodigoSaft { get; set; }

        [Required]
        [StringLength(254)]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>Norma legal aplicável (legado: Norma).</summary>
        [StringLength(254)]
        public string? Norma { get; set; }

        /// <summary>Menção obrigatória no documento (legado: Mencao).</summary>
        [StringLength(254)]
        public string? Mencao { get; set; }
    }
}
