using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Antecedentes;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Specifications
{
    public class AntecedentesCirurgicosSearchTable : Specification<AntecedentesCirurgicos>
    {
        public AntecedentesCirurgicosSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if(filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                if (Guid.TryParse(filter.Value, out Guid utenteGuid))
                                {
                                    _ = Query.Where(x => x.UtenteId == utenteGuid);
                                }
                            }
                            break;
                        case "tipocirurgia":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x => x.TipoCirurgia != null && x.TipoCirurgia.Contains(filter.Value));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if(string.IsNullOrEmpty(dynamicOrder))
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
