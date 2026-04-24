#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Domain.Entities.Exames
{
    [Table("TipoExame", Schema = "Exames")]
    public class TipoExame : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        [StringLength(100)]
        public string? Designacao {get;set;}

        public Guid? CategoriaProcedimentoId {get;set;}

        [ForeignKey("CategoriaProcedimentoId")]
        public CategoriaProcedimento? CategoriaProcedimento {get;set;}

        public Guid? TaxaIvaId {get;set;}

        [ForeignKey("TaxaIvaId")]
        public TaxaIva? TaxaIva {get;set;}
        
        [StringLength(20)]
        public string? EAN {get;set;}

        public decimal Preco {get;set;}

        public Guid? MotivoIsencaoId {get;set;}
        [ForeignKey("MotivoIsencaoId")]
        public MotivoIsencao? MotivoIsencao {get;set;}

        public string? RecomendacoesVariaveis {get;set;}

        public int? Laboratorio {get;set;}

        public bool Inativo {get;set;}

        public ICollection<GrupoAnaliseLinha> GrupoAnaliseLinhas {get;set;} = new List<GrupoAnaliseLinha>();


    }
}