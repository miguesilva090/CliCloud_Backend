using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Organismos.OrganismoService.Specifications
{
    public class OrganismoSearchTable : Specification<Organismo>
    {
        public OrganismoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Include(x => x.Rua)
            .ThenInclude(x => x.Freguesia)
            .ThenInclude(x => x.Concelho)
            .ThenInclude(x => x.Distrito)
            .ThenInclude(x => x.Pais)
            .Include(x => x.Rua)
            .ThenInclude(x => x.CodigoPostal)
            .Include(x => x.CodigoPostal)
            .Include(x => x.Freguesia)
            .Include(x => x.Concelho)
            .Include(x => x.Distrito)
            .Include(x => x.Pais)
            .Include(x => x.EntidadeContactos)
            .Include(x => x.Banco);

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
                  case "nomecomercial":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NomeComercial != null && x.NomeComercial.Contains(filter.Value));
                    }
                    break;
                  case "abreviatura":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Abreviatura != null && x.Abreviatura.Contains(filter.Value));
                    }
                    break;
                  case "numeroContribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "codigoclinica":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.CodigoClinica != null && x.CodigoClinica.Contains(filter.Value));
                    }
                    break;
                  case "categoria":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Categoria != null && x.Categoria.Contains(filter.Value));
                    }
                    break;
                  case "ars":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Ars != null && x.Ars.Contains(filter.Value));
                    }
                    break;
                  case "bancoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid bancoId))
                    {
                      _ = Query.Where(x => x.BancoId == bancoId);
                    }
                    break;
                  case "trust":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool trust))
                    {
                      _ = Query.Where(x => x.TRUST == trust);
                    }
                    break;
                  case "adm":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool adm))
                    {
                      _ = Query.Where(x => x.ADM == adm);
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
                      if(entidadeTipoId == 6) // Organismo = 6
                      {
                        _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.Organismo);
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
