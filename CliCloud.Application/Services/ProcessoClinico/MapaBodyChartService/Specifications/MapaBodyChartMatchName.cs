using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Specifications
{
    public class MapaBodyChartMatchName : Specification<MapaBodyChart>
    {
        public MapaBodyChartMatchName(string? keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(h => h.Nome.Contains(keyword));
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
