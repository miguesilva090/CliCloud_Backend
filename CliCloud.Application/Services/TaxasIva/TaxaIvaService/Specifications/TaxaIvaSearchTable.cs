using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.Specifications
{
    public class TaxaIvaSearchTable : Specification<TaxaIva>
    {
        public TaxaIvaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Descricao.Contains(filter.Value));
                            break;
                        case "taxa":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && decimal.TryParse(filter.Value, out decimal taxaVal))
                                _ = Query.Where(x => x.Taxa == taxaVal);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Descricao);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
