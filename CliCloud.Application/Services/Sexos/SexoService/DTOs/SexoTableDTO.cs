using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sexos.SexoService.DTOs
{
    public class SexoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}

