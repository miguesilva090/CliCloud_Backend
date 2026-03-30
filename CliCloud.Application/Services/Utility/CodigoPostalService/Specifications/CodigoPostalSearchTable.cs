using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.Specifications
{
    public class CodigoPostalSearchTable : Specification<CodigoPostal>
    {
        public CodigoPostalSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "codigo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Codigo.Contains(filter.Value));
                    }
                    break;
                  case "localidade":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Localidade.Contains(filter.Value));
                    }
                    break;
                  default:
                    break;
                }
              }
            }
            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Codigo); // ordem alfabética por defeito
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
