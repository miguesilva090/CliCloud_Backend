using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.IndiceMassaCorporalService.DTOs
{
    public class IndiceMassaCorporalDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

