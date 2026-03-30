using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs
{
    public class NotasBodyChartDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal XPercent { get; set; }
        public decimal YPercent { get; set; }
        public Guid MapaBodyChartId { get; set; }
        public MapaBodyChartDTO MapaBodyChart { get; set; } = null!;
        public Guid MarcadorBodyChartId { get; set; }
        public MarcadorBodyChartDTO MarcadorBodyChart { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}

