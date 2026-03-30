using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Doencas;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Specifications
{
    public class AntecedentesPessoaisSearchTable : Specification<AntecedentesPessoais>
    {
        public AntecedentesPessoaisSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value)
                                && Guid.TryParse(filter.Value, out Guid utenteId))
                            {
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            }
                            break;
                        case "nomedoenca":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x => x.NomeDoenca != null && x.NomeDoenca.Contains(filter.Value));
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
