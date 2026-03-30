#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais 
{
    [Table("GorduraMassaMuscular", Schema = "SinaisVitais")]
    public class GorduraMassaMuscular : AuditableEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        // % Gordura Corporal
        public decimal? PercentAguaCorpo { get; set; }
        public decimal? PercentGordPernaDir { get; set; }
        public decimal? PercentGordPernaEsq { get; set; }
        public decimal? PercentGordBracoDir { get; set; }
        public decimal? PercentGordBracoEsq { get; set; }
        public decimal? PercentGordTronco { get; set; }
        public decimal? GorduraVisceral { get; set; }

        // Kg Massa Muscular
        public decimal? MassaMuscPernaDir { get; set; }
        public decimal? MassaMuscPernaEsq { get; set; }
        public decimal? MassaMuscBracoDir { get; set; }
        public decimal? MassaMuscBracoEsq { get; set; }
        public decimal? MassaMuscTronco { get; set; }
        public decimal? ConsumoMetabolico { get; set; }
    }
}
