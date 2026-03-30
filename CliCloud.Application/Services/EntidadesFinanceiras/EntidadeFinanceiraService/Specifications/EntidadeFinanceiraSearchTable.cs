using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.EntidadesFinanceiras;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraSearchTable : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Include(x => x.Rua)
            .ThenInclude(x => x.Freguesia)
            .ThenInclude(x => x.Concelho)
            .ThenInclude(x => x.Distrito)
            .ThenInclude(x => x.Pais)
            .Include(x => x.TipoEntidadeFinanceira);

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
                  case "abreviatura":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Abreviatura != null && x.Abreviatura.Contains(filter.Value));
                    }
                    break;
                  case "paisprefixo":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.PaisPrefixo.Contains(filter.Value));
                    }
                    break;
                  case "numeroContribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "tipoentidadefinanceiraid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid tipoEntidadeFinanceiraId))
                    {
                      _ = Query.Where(x => x.TipoEntidadeFinanceiraId == tipoEntidadeFinanceiraId);
                    }
                    break;
                  case "condicaosns":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<CondicaoSns>(filter.Value, true, out CondicaoSns condicaoSns))
                    {
                      _ = Query.Where(x => x.CondicaoSns == condicaoSns);
                    }
                    break;
                  case "rua.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Nome.Contains(filter.Value));
                    }
                    break;
                  case "entidadetipoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int entidadeTipoId))
                    {
                      if(entidadeTipoId == 10) // EntidadeFinanceira = 10
                      {
                        _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.EntidadeFinanceira);
                      }
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
