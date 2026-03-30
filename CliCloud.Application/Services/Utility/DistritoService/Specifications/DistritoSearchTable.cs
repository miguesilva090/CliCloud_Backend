using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.DistritoService.Specifications
{
    public class DistritoSearchTable : Specification<Distrito>
    {
        public DistritoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

          _ = Query.Include(x => x.Pais);

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
                case "paisid":
                  if(Guid.TryParse(filter.Value, out Guid parsedPaisId))
                  {
                    _ = Query.Where(x => x.PaisId == parsedPaisId);
                  }
                  break;
                case "pais.nome":
                  if(!string.IsNullOrWhiteSpace(filter.Value))
                  {
                    _ = Query.Where(x => x.Pais.Nome.Contains(filter.Value));
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
