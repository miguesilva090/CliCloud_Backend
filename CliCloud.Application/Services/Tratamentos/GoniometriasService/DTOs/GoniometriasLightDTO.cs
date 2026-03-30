using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs
{
    public class GoniometriasLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
    }
}