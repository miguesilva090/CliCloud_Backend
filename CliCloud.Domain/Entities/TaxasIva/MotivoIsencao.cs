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
    public class MotivoIsencao : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;
    }
}
