using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.Specifications
{
    public class FornecedorSearchTable : Specification<Fornecedor>
    {
        public FornecedorSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Include(x => x.Rua)
            .ThenInclude(x => x.Freguesia)
            .ThenInclude(x => x.Concelho)
            .ThenInclude(x => x.Distrito)
            .ThenInclude(x => x.Pais)
            .Include(x => x.Rua)
            .ThenInclude(x => x.CodigoPostal)
            .Include(x => x.CodigoPostal)
            .Include(x => x.EntidadeContactos);

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
                  case "numeroContribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "rua.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Nome.Contains(filter.Value));
                    }
                    break;
                  case "rua.freguesia.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Freguesia != null && x.Rua.Freguesia.Nome.Contains(filter.Value));
                    }
                    break;
                  case "entidadetipoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int entidadeTipoId))
                    {
                      if(entidadeTipoId == 8) // Fornecedor = 8
                      {
                        _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.Fornecedor);
                      }
                    }
                    break;
                  case "origem":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<OrigemFornecedor>(filter.Value, out OrigemFornecedor origem))
                    {
                      _ = Query.Where(x => x.Origem == origem);
                    }
                    break;
                  case "tipofornecedor":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<TipoFornecedor>(filter.Value, out TipoFornecedor tipoFornecedor))
                    {
                      _ = Query.Where(x => x.TipoFornecedor == tipoFornecedor);
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
