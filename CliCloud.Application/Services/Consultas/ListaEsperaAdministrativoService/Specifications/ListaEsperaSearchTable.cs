using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Specifications;

public sealed class ListaEsperaSearchTable : Specification<ListaEsperaConsulta>
{
    public ListaEsperaSearchTable(ListaEsperaTableFilter filter, string? dynamicOrder = "")
    {
        _ = Query
            .Include(x => x.Utente)
            .Include(x => x.Medico)
            .Include(x => x.Especialidade)
            .Include(x => x.Organismo)
            .Include(x => x.Prioridade)
            .Include(x => x.TipoConsultaItem);

        _ = Query.Where(x => x.DeletedOn == null);

        if(!filter.IncluirConvertidos)
        {
            _ = Query.Where(x => x.ConsultaMarcacaoId == null);
        }

        if (filter.MedicoAgendaId.HasValue)
        {
            Guid medId = filter.MedicoAgendaId.Value;
            Guid? espAgenda = filter.MedicoAgendaEspecialidadeId;
            _ = Query.Where(x =>
                x.MedicoId == medId
                || (x.MedicoId == null && espAgenda != null && x.EspecialidadeId == espAgenda));
        }
        else if (filter.MedicoId.HasValue)
        {
            _ = Query.Where(x => x.MedicoId == filter.MedicoId.Value);
        }

        if(filter.UtenteId.HasValue)
        {
            _ = Query.Where(x => x.UtenteId == filter.UtenteId.Value);
        }

        if(filter.EspecialidadeId.HasValue)
        {
            _ = Query.Where(x => x.EspecialidadeId == filter.EspecialidadeId.Value);
        }

        if (filter.PrioridadeId.HasValue)
        {
            _ = Query.Where(x => x.PrioridadeId == filter.PrioridadeId.Value);
        }

        if(filter.DataDe.HasValue)
        {
            DateTime d = filter.DataDe.Value.Date;
            _ = Query.Where(x => x.Data.Date >= d);
        }

        if(filter.DataAte.HasValue)
        {
            DateTime d = filter.DataAte.Value.Date;
            _ = Query.Where(x => x.Data.Date <= d);
        }

        foreach (TableFilter f in filter.Filters ?? [])
        {
            string id =(f.Id ?? string.Empty).ToLowerInvariant();
            string? val = f.Value;
            if(string.IsNullOrWhiteSpace(val))
            {
                continue;
            }

            switch(id)
            {
                case "filtrobox":
                case "nomeutente": 
                    _ = Query.Where(x => x.Utente != null && ( (x.Utente.Nome != null && x.Utente.Nome.Contains(val))
                    || (x.Utente.NumeroUtente != null && x.Utente.NumeroUtente.Contains(val))
                    )
                    );
                    break;
                case "data":
                    if(DateTime.TryParse(val, out DateTime dt))
                    {
                        _ = Query.Where(x => x.Data.Date == dt.Date);
                    }
                    break;
                case "utenteid":
                    if(Guid.TryParse(val, out Guid utenteId))
                    {
                        _ = Query.Where(x => x.UtenteId == utenteId);
                    }
                    break;
                case "medicoid":
                    if(Guid.TryParse(val, out Guid medicoId))
                    {
                        _ = Query.Where(x => x.MedicoId == medicoId);
                    }
                    break;
                case "especialidadeid":
                    if(Guid.TryParse(val, out Guid especialidadeId))
                    {
                        _ = Query.Where(x => x.EspecialidadeId == especialidadeId);
                    }
                    break;
                case "prioridadeid":
                    if(Guid.TryParse(val, out Guid prioridadeId))
                    {
                        _ = Query.Where(x => x.PrioridadeId == prioridadeId);
                    }
                    break;
                case "credencial":
                    _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
                    break;
            }
        }

        if(string.IsNullOrWhiteSpace(dynamicOrder))
        {
            _ = Query.OrderByDescending(x => x.PrioridadeId).ThenBy(x => x.Data).ThenBy(x => x.CreatedOn);
        }
        else
        {
            _ = Query.OrderBy(dynamicOrder);
        }
    }
}