using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Specifications
{
    public class MapaBodyChartByIdWithMarkers : Specification<MapaBodyChart>
    {
        public MapaBodyChartByIdWithMarkers(Guid id)
        {
            _ = Query.Where(x => x.Id == id);
            _ = Query.Include(x => x.Marcadores);
        }
    }
}

