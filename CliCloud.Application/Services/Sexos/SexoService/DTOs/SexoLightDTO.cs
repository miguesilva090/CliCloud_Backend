using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sexos.SexoService.DTOs
{
    public class SexoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}

