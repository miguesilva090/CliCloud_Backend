using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Specifications
{
    public class GorduraMassaMuscularSearchTable : Specification<GorduraMassaMuscular>
    {
        public GorduraMassaMuscularSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid utenteId))
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
