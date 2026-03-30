using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs
{
    public class MotivoAltaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; } = string.Empty;
    }
}