using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Consultas;


namespace CliCloud.Domain.Entities.ProcessoClinico.Odontologia 
{
    [Table("OdontogramaDefinitivo", Schema = "Odontologia")]
    public class OdontogramaDefinitivo : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        [Required]
        public Guid ConsultaId {get;set;}
        public Consulta Consulta {get;set;} = null!;

        [Required]
        public int NumeroDente {get;set;}

        public int? NumeroDenteAte {get;set;}

        [MaxLength(50)]
        public string? CodigoSuperficie {get;set;}

        [MaxLength(10)]
        public string? CodigoEstadoPadrao {get;set;}

        [MaxLength(10)]
        public string? CodigoTratamentoPadrao {get;set;}

        [MaxLength(10)]
        public string? CodigoEstadoPersonalizado {get;set;}

        [MaxLength(10)]
        public string? CodigoTratamentoPersonalizado {get;set;}

        [Required]
        [MaxLength(200)]
        public string Descricao {get;set;} = null!;

        public string? Observacoes {get;set;}

        public bool Faturar {get;set;}
        public int Quantidade {get;set;} = 1;

        public decimal? ValorServico {get;set;}
        public decimal? ValorUtente {get;set;}
        public decimal? ValorEntidade {get;set;}

        public Guid? LinhaFaturacaoId {get;set;}

        [ForeignKey(nameof(CodigoEstadoPadrao))]
        public EstadosDentarios? EstadoPadrao {get;set;}

        [ForeignKey(nameof(CodigoTratamentoPadrao))]
        public TipoTratamentoDentario? TiposTratamentoPadrao {get;set;}

    }
}