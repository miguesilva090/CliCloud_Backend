using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs
{
    public class GorduraMassaMuscularLightDTO : IDto
    {
        public Guid UtenteId { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public decimal? PercentAguaCorpo { get; set; }

        public decimal? GorduraVisceral { get; set; }

        public decimal? ConsumoMetabolico { get; set; }
    }
}
