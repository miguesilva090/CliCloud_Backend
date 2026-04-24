using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("AnamneseOrtodonticaATM", Schema = "Estomatologia")]
    public class AnamneseOrtodonticaATM : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        public string? Palpacao {get;set;}
        public string? RelacaoCentrica {get;set;}
        public string? LateralidadeEsquerda {get;set;}
        public string? LateralidadeDireita {get;set;}
        public string? Protrusao {get;set;}
        public string? MusculosMastigatorios {get;set;}
        public string? MusculosInfra {get;set;}
        public string? MusculosSupra {get;set;}
        public string? Obs {get;set;}

        public bool? ApertaOuRangeDentes {get;set;}
        public bool? MusculosMandibulaDoridosAoAcordar {get;set;}
        public bool? DorMandibulaOuvido {get;set;}
        public bool? NaoPodeAbrirFecharBoca {get;set;}
        public bool? DentesSensiveisDesgastados {get;set;}
        public bool? SofreuAlgumTraumatismo {get;set;}
        public bool? SenteBarulhoZumbido {get;set;}
    }
}