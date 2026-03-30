using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;


namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Specifications
{
    public class MapaBodyChartSearchList : Specification<MapaBodyChart>
    {
        public MapaBodyChartSearchList(string? keyword = "")
        {
            // exclude soft-deleted records
            _ = Query.Where(x => x.DeletedOn == null);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
