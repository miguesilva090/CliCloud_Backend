using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("AnamneseOrtodonticaAnaliseGeral", Schema = "Estomatologia")]
    public class AnamneseOrtodonticaAnaliseGeral : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        public string? SimetriaFacial {get;set;}
        public string? DesenvolvimentoMaxila {get;set;}
        public string? DesenvolvimentoMandibula {get;set;}
        public string? PerfilFacial {get;set;}
        public string? AlturaFacialInferior {get;set;}
        public string? CaracteristicasLabios {get;set;}
        public string? RelacaoLabioDenteSuperior {get;set;}
        public string? RelacaoLabioDenteInferior {get;set;}
        public string? FreioLingual {get;set;}
        public string? FormaNariz {get;set;}
        public string? TecidosMolesIntrabucais {get;set;}

        public int? TipoFacial {get;set;}
        public int? DistanciaIntercomissuralNasal {get;set;}
        public int? DistanciaIntercomissuralPupilar {get;set;}
        public int? Adenoides {get;set;}
        public int? Amigdalas {get;set;}

        
      
    }
}