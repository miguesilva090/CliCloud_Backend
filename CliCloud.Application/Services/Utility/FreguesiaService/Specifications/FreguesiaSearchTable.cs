using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utility;


namespace CliCloud.Application.Services.Utility.FreguesiaService.Specifications
{
    public class FreguesiaSearchTable : Specification<Freguesia>
    {
        public FreguesiaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

          _ = Query.Include(x => x.Concelho).ThenInclude(x => x.Distrito).ThenInclude(x => x.Pais);

            // filters
            if (filters != null && filters.Count != 0)
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
                  case "concelhoid":
                    if(Guid.TryParse(filter.Value, out Guid concelhoId))
                    {
                      _ = Query.Where(x => x.ConcelhoId == concelhoId);
                    }
                    break;
                  case "concelho.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Concelho.Nome.Contains(filter.Value));
                    }
                    break;
                  case "concelho.distrito.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Concelho.Distrito.Nome.Contains(filter.Value));
                    }
                    break;
                  case "concelho.distrito.pais.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Concelho.Distrito.Pais.Nome.Contains(filter.Value));
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
