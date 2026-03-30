using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.AlergiaUtenteService.Specifications
{
    public class AlergiaUtenteSearchTable : Specification<AlergiaUtente>
    {
        public AlergiaUtenteSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.Alergia);

            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (Guid.TryParse(filter.Value, out Guid utenteId))
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            break;
                        case "observacoes":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Observacoes != null && x.Observacoes.Contains(filter.Value));
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderByDescending(x => x.CreatedOn);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
