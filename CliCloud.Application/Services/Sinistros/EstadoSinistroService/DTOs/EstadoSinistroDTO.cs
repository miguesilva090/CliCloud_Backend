using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs
{
    public class EstadoSinistroDTO : IDto 
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}