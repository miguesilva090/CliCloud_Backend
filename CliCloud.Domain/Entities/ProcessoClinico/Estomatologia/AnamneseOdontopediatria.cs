using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("AnamneseOdontopediatria", Schema = "Estomatologia")]
    public class AnamneseOdontopediatria : AuditableEntity
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        public decimal? Peso {get;set;}
        public decimal? Altura {get;set;}

        public decimal? PesoPai {get;set;}
        public decimal? AlturaPai {get;set;}

        public decimal? PesoMae {get;set;}
        public decimal? AlturaMae {get;set;}

        public int? CaracteristicasGeraisDesenvolvimento {get;set;}
        public int? TipoAmamentacao {get;set;}
        public int? ObservacaoCardiaca {get;set;}
        public int? ObservacaoRespiracao {get;set;}
        public int? ObservacaoDiccao {get;set;}

        public string? ObservacoesAdicionais {get;set;}
    }
}