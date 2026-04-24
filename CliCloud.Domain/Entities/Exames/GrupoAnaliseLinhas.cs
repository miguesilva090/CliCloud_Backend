#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Exames
{
    /// <summary>
    /// Linha do grupo de análises de um TipoExame: associa uma Analise ao tipo de exame.
    /// Opcionalmente permite overrides de descrição/unidade/valores de referência para esta linha.
    /// </summary>
    [Table("GrupoAnaliseLinha", Schema = "Exames")]
    public class GrupoAnaliseLinha : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid TipoExameId { get; set; }
        [ForeignKey("TipoExameId")]
        public TipoExame TipoExame { get; set; } = null!;

        [Required]
        public Guid AnaliseId { get; set; }
        [ForeignKey("AnaliseId")]
        public Analises Analise { get; set; } = null!;

        public int Ordem { get; set; }

        [StringLength(200)]
        public string? Descricao { get; set; }

        [StringLength(50)]
        public string? UnidadeMedida { get; set; }

        [StringLength(200)]
        public string? ValoresReferencia { get; set; }
    }
}
