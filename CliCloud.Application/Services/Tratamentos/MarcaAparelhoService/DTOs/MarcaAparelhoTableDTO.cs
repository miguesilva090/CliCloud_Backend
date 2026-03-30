using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs
{
    public class MarcaAparelhoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Designacao { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

