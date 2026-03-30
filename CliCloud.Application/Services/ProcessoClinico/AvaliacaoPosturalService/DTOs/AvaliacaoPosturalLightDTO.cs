using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs
{
    public class AvaliacaoPosturalLightDTO : IDto
    {
        public Guid UtenteId { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }
    }
}
