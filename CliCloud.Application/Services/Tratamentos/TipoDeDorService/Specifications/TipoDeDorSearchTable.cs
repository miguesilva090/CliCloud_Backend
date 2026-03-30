using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.Specifications
{
    public class TipoDeDorSearchTable : Specification<TipoDeDor>
    {
        public TipoDeDorSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
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
                        default:
                            break;
                    }
                }
            }


            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Descricao); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }


        }
    }
}
