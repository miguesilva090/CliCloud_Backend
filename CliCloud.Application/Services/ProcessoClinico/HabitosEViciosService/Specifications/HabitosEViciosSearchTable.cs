using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Specifications
{
    public class HabitosEViciosSearchTable : Specification<HabitosEVicios>
    {
        public HabitosEViciosSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    switch ((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value)
                                && Guid.TryParse(filter.Value, out var utenteId))
                            {
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}
