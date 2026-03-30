using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.PaisService.Specifications
{
    public class PaisSearchTable : Specification<Pais>
    {
        public PaisSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

           if(filters != null && filters.Count != 0)
           {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Nome.Contains(filter.Value));
                    }
                    break;
                  case "codigo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Codigo.Contains(filter.Value));
                    }
                    break;
                  case "prefixo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Prefixo.Contains(filter.Value));
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
                _ = Query.OrderBy(x => x.Nome); // ordem alfabética por defeito
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
