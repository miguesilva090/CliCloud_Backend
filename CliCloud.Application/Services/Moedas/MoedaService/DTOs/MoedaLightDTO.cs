using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Moedas.MoedaService.DTOs
{
    public class MoedaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Abreviatura { get; set; }
        public string? Simbolo { get; set; }
    }
}
