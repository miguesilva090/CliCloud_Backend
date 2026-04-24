#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais 
{
    [Table("TemperaturaCorporal", Schema = "SinaisVitais")]
    public class TemperaturaCorporal : AuditableEntityWithSoftDelete 
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public decimal Temperatura { get; set; }

        public string? Observacoes { get; set; }
    }
}