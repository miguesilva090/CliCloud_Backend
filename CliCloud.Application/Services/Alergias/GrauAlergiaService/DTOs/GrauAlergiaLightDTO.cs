using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrauAlergiaService.DTOs
{
    public class GrauAlergiaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
