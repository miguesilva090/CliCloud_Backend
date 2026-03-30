using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs
{
    public class MotivoAltaDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

