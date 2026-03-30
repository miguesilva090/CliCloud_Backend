#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais 
{
    [Table("AvaliacaoAntropometrica", Schema = "SinaisVitais")]
    public class AvaliacaoAntropometrica : AuditableEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public decimal? QuadricepEsq { get; set; }
        public decimal? QuadricepDir { get; set; }
        public decimal? QuadricepDif { get; set; }
        public decimal? IsquiotibialEsq { get; set; }
        public decimal? IsquiotibialDir { get; set; }
        public decimal? IsquiotibialDif { get; set; }
        public decimal? AdutorEsq { get; set; }
        public decimal? AdutorDir { get; set; }
        public decimal? AdutorDif { get; set; }
        public decimal? AbdutorEsq { get; set; }
        public decimal? AbdutorDir { get; set; }
        public decimal? AbdutorDif { get; set; }
        public decimal? GluteoEsq { get; set; }
        public decimal? GluteoDir { get; set; }
        public decimal? GluteoDif { get; set; }
        public decimal? GemeoEsq { get; set; }
        public decimal? GemeoDir { get; set; }
        public decimal? GemeoDif { get; set; }
        public decimal? AbdutorOmbroEsq { get; set; }
        public decimal? AbdutorOmbroDir { get; set; }
        public decimal? AbdutorOmbroDif { get; set; }
        public decimal? FlexorOmbroEsq { get; set; }
        public decimal? FlexorOmbroDir { get; set; }
        public decimal? FlexorOmbroDif { get; set; }
        public decimal? ExtensorOmbroEsq { get; set; }
        public decimal? ExtensorOmbroDir { get; set; }
        public decimal? ExtensorOmbroDif { get; set; }
        public decimal? RotadorInternoOmbroEsq { get; set; }
        public decimal? RotadorInternoOmbroDir { get; set; }
        public decimal? RotadorInternoOmbroDif { get; set; }
        public decimal? RotadorExternoOmbroEsq { get; set; }
        public decimal? RotadorExternoOmbroDir { get; set; }
        public decimal? RotadorExternoOmbroDif { get; set; }
    }
}
