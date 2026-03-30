using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs
{
    public class MarcaAparelhoDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Designacao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

