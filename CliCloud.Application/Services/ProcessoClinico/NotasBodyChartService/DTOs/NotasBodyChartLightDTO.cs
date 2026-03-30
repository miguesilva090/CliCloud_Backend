using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs
{
    public class NotasBodyChartLightDTO : IDto 
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        
    }
}