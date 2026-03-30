using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Specifications
{
    public class HorarioTecnicoDiaSearchTable : Specification<HorarioTecnicoDia>
    {
        public HorarioTecnicoDiaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.HorarioTecnico)
                .ThenInclude(x => x.Tecnico);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "horariotecnico.tecnico.nome":
                  case "horariotecnicotecnonome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.HorarioTecnico != null && x.HorarioTecnico.Tecnico != null && x.HorarioTecnico.Tecnico.Nome.Contains(filter.Value));
                    }
                    break;
                  case "horariotecnicoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid horarioTecnicoId))
                    {
                      _ = Query.Where(x => x.HorarioTecnicoId == horarioTecnicoId);
                    }
                    break;
                  case "diasemana":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<DiaSemana>(filter.Value, out DiaSemana diaSemana))
                    {
                      _ = Query.Where(x => x.DiaSemana == diaSemana);
                    }
                    break;
                  case "periodo":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<Periodo>(filter.Value, out Periodo periodo))
                    {
                      _ = Query.Where(x => x.Periodo == periodo);
                    }
                    break;
                  case "sala":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Sala != null && x.Sala.Contains(filter.Value));
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
                _ = Query.OrderBy(x => x.DiaSemana)
                    .ThenBy(x => x.Periodo)
                    .ThenBy(x => x.Inicio); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
