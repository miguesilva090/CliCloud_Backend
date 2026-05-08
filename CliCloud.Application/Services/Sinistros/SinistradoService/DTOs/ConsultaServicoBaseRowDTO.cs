using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class ConsultaServicoBaseRowDTO : IDto
    {
        public Guid Id { get; set; }
        public DateTime? Data { get; set; }
        public Guid? AdmissaoId { get; set; }
        public string? TipoConsultaDesignacao { get; set; }
    }
}
