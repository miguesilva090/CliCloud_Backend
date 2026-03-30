using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Specifications
{
    public class NotasBodyChartSearchTable : Specification<NotaBodyChart>
    {
        public NotasBodyChartSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "nome":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Titulo != null && x.Titulo.Contains(filter.Value));
                            break;
                        case "mapabodychartid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid mapaId))
                                _ = Query.Where(x => x.MapaBodyChartId == mapaId);
                            break;
                        case "tratamentoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid tratId))
                                _ = Query.Where(x => x.TratamentoId == tratId);
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
