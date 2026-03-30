using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.RuaService.Specifications
{
    public class RuaSearchTable : Specification<Rua>
    {
        public RuaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Include(f => f.Freguesia).ThenInclude(c => c.Concelho).ThenInclude(d => d.Distrito).ThenInclude(p => p.Pais);
          _ = Query.Include(c => c.CodigoPostal);

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
                  case "freguesiaid":
                    if(Guid.TryParse(filter.Value, out Guid FreguesiaId))
                    {
                      _ = Query.Where(x => x.FreguesiaId == FreguesiaId);
                    }
                    break;
                  case "freguesia.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Freguesia.Nome.Contains(filter.Value));
                    }
                    break;
                  case "codigoPostalId":
                    if(Guid.TryParse(filter.Value, out Guid CodigoPostalId))
                    {
                      _ = Query.Where(x => x.CodigoPostalId == CodigoPostalId);
                    }
                    break;
                  case "codigoPostal.codigo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.CodigoPostal.Codigo.Contains(filter.Value));
                    }
                    break;
                  case "freguesia.concelho.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Freguesia.Concelho.Nome.Contains(filter.Value));
                    }
                    break;
                  case "freguesia.concelho.distrito.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Freguesia.Concelho.Distrito.Nome.Contains(filter.Value));
                    }
                    break;
                  case "freguesia.concelho.distrito.pais.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Freguesia.Concelho.Distrito.Pais.Nome.Contains(filter.Value));
                    }
                    break;
                  case "freguesia.concelho.distrito.pais.codigo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Freguesia.Concelho.Distrito.Pais.Codigo.Contains(filter.Value));
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
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }


        }
    }
}
