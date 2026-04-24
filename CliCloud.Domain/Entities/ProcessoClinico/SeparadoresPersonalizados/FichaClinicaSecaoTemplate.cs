#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados
{
    [Table("FichaClinicaSecaoTemplate", Schema = "ProcessoClinico")]
    public class FichaClinicaSecaoTemplate : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        public Guid UtilizadorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descricao { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; } = true;
    }
}