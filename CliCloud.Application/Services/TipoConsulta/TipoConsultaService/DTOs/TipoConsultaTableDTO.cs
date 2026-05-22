using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs
{
    public class TipoConsultaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public int? CodigoLegado { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
