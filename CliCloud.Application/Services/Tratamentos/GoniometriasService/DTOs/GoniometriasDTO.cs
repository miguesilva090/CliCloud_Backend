using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs
{
    public class GoniometriasDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

