using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.Specifications
{
    public class EspecialidadeSearchTable : Specification<Especialidade>
    {
        public EspecialidadeSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.CategoriaEspecialidade);

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
                  case "categoriaespecialidade.descricao":
                  case "categoriaespecialidadedescricao":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.CategoriaEspecialidade != null && x.CategoriaEspecialidade.Descricao.Contains(filter.Value));
                    }
                    break;
                  case "fisioterapia":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool fisioterapia))
                    {
                      _ = Query.Where(x => x.Fisioterapia == fisioterapia);
                    }
                    break;
                  case "atendimento":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool atendimento))
                    {
                      _ = Query.Where(x => x.Atendimento == atendimento);
                    }
                    break;
                  case "globalbooking":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool globalbooking))
                    {
                      _ = Query.Where(x => x.Globalbooking == globalbooking);
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
                _ = Query.OrderBy(x => x.Nome); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
