using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using TipoEntidadeFinanceiraEntity = CliCloud.Domain.Entities.TipoEntidadeFinanceira.TipoEntidadeFinanceira;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Specifications
{
    public class TipoEntidadeFinanceiraSearchTable : Specification<TipoEntidadeFinanceiraEntity>
    {
        public TipoEntidadeFinanceiraSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "sigla":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Sigla.Contains(filter.Value));
                    }
                    break;
                  case "designacao":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Designacao.Contains(filter.Value));
                    }
                    break;
                  case "dominio":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Dominio.Contains(filter.Value));
                    }
                    break;
                  case "descricaodominio":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.DescricaoDominio.Contains(filter.Value));
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
                _ = Query.OrderBy(x => x.Designacao); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
