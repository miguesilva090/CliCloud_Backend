using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs
{
    public class TipoConsultaDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public int? CodigoLegado { get; set; }
    }
}
