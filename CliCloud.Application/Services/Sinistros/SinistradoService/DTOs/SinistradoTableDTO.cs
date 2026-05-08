using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class SinistradoTableDTO : IDto 
    {
        public Guid Id { get; set; }
        public string CodigoSinistro { get; set; } = string.Empty;
        public Guid UtenteId { get; set; }
        public DateTime? DataAcidente { get; set; }
        public string? EstadoSinistroDesignacao { get; set; }
        public bool Historico { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}