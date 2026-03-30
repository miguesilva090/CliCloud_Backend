using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs
{
    public class MapaBodyChartLightDTO : IDto 
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? CaminhoImagem { get; set; }
    }
}