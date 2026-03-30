using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Specifications
{
    public class HorarioTecnicoSearchTable : Specification<HorarioTecnico>
    {
        public HorarioTecnicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.Tecnico)
                .Include(x => x.Horarios);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "tecnico.nome":
                  case "tecniconome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Tecnico != null && x.Tecnico.Nome.Contains(filter.Value));
                    }
                    break;
                  case "tecnico.numeroContribuinte":
                  case "tecniconumerocontribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Tecnico != null && x.Tecnico.NumeroContribuinte != null && x.Tecnico.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "tecnicoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid tecnicoId))
                    {
                      _ = Query.Where(x => x.TecnicoId == tecnicoId);
                    }
                    break;
                  case "tipohorario":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int tipoHorario))
                    {
                      _ = Query.Where(x => x.TipoHorario == tipoHorario);
                    }
                    break;
                  case "horacomp":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int horaComp))
                    {
                      _ = Query.Where(x => x.HoraComp == horaComp);
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
