using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs
{
    public class MapaBodyChartTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? CaminhoImagem { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}