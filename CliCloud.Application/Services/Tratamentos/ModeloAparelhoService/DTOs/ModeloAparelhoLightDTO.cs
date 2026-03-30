#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs
{
    public class ModeloAparelhoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Designacao { get; set; }
        public Guid MarcaAparelhoId { get; set; }
        public string? MarcaAparelhoDesignacao { get; set; }
    }
}

