using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs
{
    public class MarcadorBodyChartDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MapaBodyChartId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string CorHex { get; set; } = string.Empty;
    }
}

