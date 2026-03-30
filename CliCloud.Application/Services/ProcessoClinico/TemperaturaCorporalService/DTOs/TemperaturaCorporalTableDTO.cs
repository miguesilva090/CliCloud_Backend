using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TemperaturaCorporalService.DTOs
{
    public class TemperaturaCorporalTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal Temperatura { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

