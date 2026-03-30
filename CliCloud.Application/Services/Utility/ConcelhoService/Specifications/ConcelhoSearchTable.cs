using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.ConcelhoService.Specifications
{
    public class ConcelhoSearchTable : Specification<Concelho>
    {
        public ConcelhoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

          _ = Query.Include(x => x.Distrito).ThenInclude(x => x.Pais);

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
                case "distritoid":
                  if(Guid.TryParse(filter.Value, out Guid parsedDistritoId))
                  {
                    _ = Query.Where(x => x.DistritoId == parsedDistritoId);
                  }
                  break;
                case "distrito.nome":
                  if(!string.IsNullOrWhiteSpace(filter.Value))
                  {
                    _ = Query.Where(x => x.Distrito.Nome.Contains(filter.Value));
                  }
                  break;
                case "distrito.pais.nome":
                  if(!string.IsNullOrWhiteSpace(filter.Value))
                  {
                    _ = Query.Where(x => x.Distrito.Pais.Nome.Contains(filter.Value));
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
