#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados
{
    [Table("FichaClinicaSecaoCampo", Schema = "ProcessoClinico")]
    public class FichaClinicaSecaoCampo : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid SeparadorId { get; set; }

        public FichaClinicaSecaoTemplate Separador { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de campo (ex.: "Texto", "TextoMultilinha"). Mantido como string para flexibilidade.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TipoCampo { get; set; } = "Texto";

    /// <summary>
    /// Número de linhas sugerido para o campo (usado em textareas).
    /// </summary>
    public int NumeroLinhas { get; set; } = 1;

        public int Ordem { get; set; }

        public bool Ativo { get; set; } = true;
    }
}

