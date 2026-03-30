using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Estomatologia
{
    [Table("AnamneseOrtodonticaDenticaoDeciduaeMista", Schema = "Estomatologia")]
    public class AnamneseOrtodonticaDenticaoDeciduaeMista : AuditableEntity
    {
        [Key]
        public new Guid Id {get;set;}

        public Guid UtenteId {get;set;}
        public Utente Utente {get;set;} = null!;

        public int? RelacaoMolarDecidua {get;set;}
        public string? DentaduraMista {get;set;}
        public string? SequenciaEsfoliacao {get;set;}
        public string? SequenciaErupcao {get;set;}
        public string? EstagioCalcificacao {get;set;}
    }
}