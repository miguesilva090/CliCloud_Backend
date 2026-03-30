using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs
{
    public class AvaliacaoAntropometricaLightDTO : IDto
    {
        public Guid UtenteId { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public decimal? QuadricepEsq { get; set; }

        public decimal? QuadricepDir { get; set; }

        public decimal? QuadricepDif { get; set; }

        public decimal? IsquiotibialEsq { get; set; }

        public decimal? IsquiotibialDir { get; set; }

        public decimal? IsquiotibialDif { get; set; }


    }
}
