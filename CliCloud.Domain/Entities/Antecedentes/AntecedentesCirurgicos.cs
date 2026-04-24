#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Antecedentes
{
    [Table("AntecedentesCirurgicos", Schema = "Antecedentes")]
    public class AntecedentesCirurgicos : AuditableEntityWithSoftDelete
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public int? Ano {get;set;}

        public string? TipoCirurgia {get;set;}

        public bool? HouveComplicacoes {get;set;}
        public string? Complicacoes {get;set;}

        public string? Observacoes {get;set;}
    }
}