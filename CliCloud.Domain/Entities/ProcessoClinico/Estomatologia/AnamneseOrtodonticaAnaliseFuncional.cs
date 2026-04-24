using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("AnamneseOrtodonticaAnaliseFuncional", Schema = "Estomatologia")]
    public class AnamneseOrtodonticaAnaliseFuncional : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        public string? LabioSuperior {get;set;}
        public string? LabioSuperiorTonicidade {get;set;}

        public string? LabioInferior {get;set;}
        public string? LabioInferiorTonicidade {get;set;}

        public string? AspetoLabioSuperiorInferior {get;set;}

        public string? LinguaAspeto {get;set;}
        public string? LinguaTonicidade {get;set;}
        public string? LinguaPosicionamento {get;set;}

        public string? MusculaturaFacial {get;set;}
        public string? MusculaturaMentoniana {get;set;}
        
        public string? TipoRespiracao {get;set;}
        public string? Forracao {get;set;}
        public string? Mastigacao {get;set;}
        public string? MusculosMastigatorios {get;set;}
        public string? ComentariosAdicionais {get;set;}
    }
}