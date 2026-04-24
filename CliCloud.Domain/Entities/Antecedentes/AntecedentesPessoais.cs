#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Antecedentes
{
    [Table("AntecedentesPessoais", Schema = "Antecedentes")]
    public class AntecedentesPessoais : AuditableEntityWithSoftDelete
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid? DoencaId {get;set;}
        public Doenca? Doenca {get;set;}
        public string? NomeDoenca {get;set;}

        public int? Ano {get;set;}

        public int? Idade {get;set;}

        public DateTime? Data {get;set;}


    }
}