using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.Specifications
{
    public class HorarioMedicoSearchTable : Specification<HorarioMedico>
    {
        public HorarioMedicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.Medico)
                .Include(x => x.Horarios);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "medico.nome":
                  case "mediconome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Medico != null && x.Medico.Nome.Contains(filter.Value));
                    }
                    break;
                  case "medico.numeroContribuinte":
                  case "mediconumerocontribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Medico != null && x.Medico.NumeroContribuinte != null && x.Medico.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "medicoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid medicoId))
                    {
                      _ = Query.Where(x => x.MedicoId == medicoId);
                    }
                    break;
                  case "tipohorario":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int tipoHorario))
                    {
                      _ = Query.Where(x => x.TipoHorario == tipoHorario);
                    }
                    break;
                  case "horacomp":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool horaComp))
                    {
                      _ = Query.Where(x => x.HoraComp == horaComp);
                    }
                    break;
                  case "horarioflexivel":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool horarioFlexivel))
                    {
                      _ = Query.Where(x => x.HorarioFlexivel == horarioFlexivel);
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
