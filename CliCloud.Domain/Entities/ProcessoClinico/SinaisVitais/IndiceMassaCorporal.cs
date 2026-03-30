#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais
{
    [Table("IndiceMassaCorporal", Schema = "SinaisVitais")]
    public class IndiceMassaCorporal : AuditableEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

        public string? Observacoes { get; set; }

    }
}