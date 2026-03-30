using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs
{
    public class EstadoCivilLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
