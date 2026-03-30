using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Specifications
{
    public class EmpresaSearchTable : Specification<Empresa>
    {
        public EmpresaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
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

            if (filters != null && filters.Count != 0)
            {
              foreach (TableFilter filter in filters)
              {
                switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "nome":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Nome.Contains(filter.Value));
                    }
                    break;
                  case "numerocontribuinte":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "rua.nome":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Nome.Contains(filter.Value));
                    }
                    break;
                  case "rua.freguesia.nome":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Freguesia != null && x.Rua.Freguesia.Nome.Contains(filter.Value));
                    }
                    break;
                  case "entidadetipoid":
                    if (!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int entidadeTipoId))
                    {
                      if (entidadeTipoId == 9) // Empresa = 9
                      {
                        _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.Empresa);
                      }
                    }
                    break;
                  default:
                    break;
                }
              }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}

