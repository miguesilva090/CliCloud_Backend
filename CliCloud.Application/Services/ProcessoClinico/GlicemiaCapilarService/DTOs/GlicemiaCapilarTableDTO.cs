using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GlicemiaCapilarService.DTOs
{
    public class GlicemiaCapilarTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public int Glicemia { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

