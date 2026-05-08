using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class TratamentoServicoBaseRowDTO : IDto
    {
        public Guid Id { get; set; }
        public DateTime? DataServico { get; set; }
        public string? Designacao { get; set; }
    }
}
