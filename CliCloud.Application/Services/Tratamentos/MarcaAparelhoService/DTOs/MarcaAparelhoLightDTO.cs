using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs
{
    public class MarcaAparelhoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Designacao { get; set; }
    }
}

