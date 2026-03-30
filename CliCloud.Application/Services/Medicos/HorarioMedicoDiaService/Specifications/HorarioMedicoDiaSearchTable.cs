using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Specifications
{
    public class HorarioMedicoDiaSearchTable : Specification<HorarioMedicoDia>
    {
        public HorarioMedicoDiaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.HorarioMedico)
                .ThenInclude(x => x.Medico);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "horariomedico.medico.nome":
                  case "horariomedicomediconome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.HorarioMedico != null && x.HorarioMedico.Medico != null && x.HorarioMedico.Medico.Nome.Contains(filter.Value));
                    }
                    break;
                  case "horariomedicoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid horarioMedicoId))
                    {
                      _ = Query.Where(x => x.HorarioMedicoId == horarioMedicoId);
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
