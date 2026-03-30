using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Specifications
{
    public class MapaBodyChartSearchTable : Specification<MapaBodyChart>
    {
        public MapaBodyChartSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            // exclude soft-deleted records
            _ = Query.Where(x => x.DeletedOn == null);

            // filters
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "nome":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Nome.Contains(filter.Value));
                            break;
                        case "caminhoimagem":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.CaminhoImagem.Contains(filter.Value));
                            break;
                        default:
                            break;
                    }
                }
            }

            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
