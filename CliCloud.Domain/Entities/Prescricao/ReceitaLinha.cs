#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Prescricao
{
    [Table("ReceitaLinha", Schema = "Prescricao")]
    public class ReceitaLinha : AuditableEntityWithSoftDelete
    {
        [Required] public Guid ReceitaMedicaId { get; set; }
        [ForeignKey(nameof(ReceitaMedicaId))] public ReceitaMedica ReceitaMedica { get; set; } = null!;

        public int Ordem { get; set; }
        public int TipoLinha { get; set; } = 1;

        [StringLength(50)] public string? EmbId { get; set; }
        [StringLength(50)] public string? Cnpem { get; set; }
        [StringLength(500)] public string Designacao { get; set; } = string.Empty;
        [StringLength(500)] public string? DescricaoEmbalagem { get; set; }

        public int Quantidade { get; set; } = 1;
        public decimal? Pvp { get; set; }
        public decimal? Comparticipacao { get; set; }
        public decimal? ValorUtente { get; set; }

        /// <summary>Texto derivado para UI / fallback (ex.: "1 comprimido (de 8/8h) | 7 Dia").</summary>
        [StringLength(1000)] public string? Posologia { get; set; }

        [StringLength(50)] public string? PosologiaQuantidadeUnidade { get; set; }
        [StringLength(50)] public string? PosologiaQuantidadeValor { get; set; }
        [StringLength(50)] public string? PosologiaFrequenciaUnidade { get; set; }
        [StringLength(50)] public string? PosologiaFrequenciaValor { get; set; }
        [StringLength(50)] public string? PosologiaDuracaoUnidade { get; set; }
        [StringLength(50)] public string? PosologiaDuracaoValor { get; set; }
        [StringLength(1000)] public string? PosologiaInstrucoes { get; set; }

        public int? CodValidade { get; set; }
        public DateTime? DataValidade { get; set; }
        [StringLength(10)] public string? CodJustificacaoQuantidade { get; set; }
        [StringLength(500)] public string? JustificacaoQuantidade { get; set; }

        /// <summary>1 = por nome/marca · 2 = por DCI (legado CodigoTipoPrescricao).</summary>
        public int? CodTipoPrescricao { get; set; }

        /// <summary>Motivo 1–4 quando prescrição por nome (CodTipoPrescricao != 2).</summary>
        public int? CodMotivo { get; set; }

        /// <summary>Indicação terapêutica 1–7 (canábis / tipoLinha == 2) — P1.3b.</summary>
        public int? CodIndicacaoTerapeutica { get; set; }

        /// <summary>Diploma/despacho Infarmed (normaRegimeExcecional / regimeExcecional).</summary>
        [StringLength(500)] public string? Diploma { get; set; }
    }
}