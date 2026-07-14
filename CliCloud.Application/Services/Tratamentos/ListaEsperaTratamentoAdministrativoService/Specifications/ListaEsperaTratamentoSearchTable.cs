using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Filters;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;

public sealed class ListaEsperaTratamentoSearchTable : Specification<ListaEsperaTratamento>
{
    public ListaEsperaTratamentoSearchTable(ListaEsperaTratamentoTableFilter filter, string? dynamicOrder = "")
    {
        _ = Query
            .Include(x => x.Utente)
            .Include(x => x.Organismo)
            .Include(x => x.Prioridade)
            .Include(x => x.EstadoListaEspera)
            .Include(x => x.LocalTratamento);

        _ = Query.Where(x => x.DeletedOn == null);
        _ = Query.Where(x => x.Historico == filter.Historico);

        if (filter.UtenteId.HasValue)
        {
            _ = Query.Where(x => x.UtenteId == filter.UtenteId.Value);
        }

        if (filter.MedicoId.HasValue)
        {
            _ = Query.Where(x => x.MedicoId == filter.MedicoId.Value);
        }

        if (filter.PrioridadeId.HasValue)
        {
            _ = Query.Where(x => x.PrioridadeId == filter.PrioridadeId.Value);
        }

        if (filter.LocalTratamentoId.HasValue)
        {
            _ = Query.Where(x => x.LocalTratamentoId == filter.LocalTratamentoId.Value);
        }

        if (filter.EstadoListaEsperaId.HasValue)
        {
            _ = Query.Where(x => x.EstadoListaEsperaId == filter.EstadoListaEsperaId.Value);
        }

        foreach (TableFilter f in filter.Filters ?? [])
        {
            string id = (f.Id ?? string.Empty).ToLowerInvariant();
            string? val = f.Value;
            if (string.IsNullOrWhiteSpace(val))
            {
                continue;
            }

            switch (id)
            {
                case "filtrobox":
                case "nomeutente":
                    _ = Query.Where(x =>
                        x.Utente != null
                        && (
                            (x.Utente.Nome != null && x.Utente.Nome.Contains(val))
                            || (x.Utente.NumeroUtente != null && x.Utente.NumeroUtente.Contains(val))
                        )
                    );
                    break;
                case "designacao":
                    _ = Query.Where(x => x.Designacao != null && x.Designacao.Contains(val));
                    break;
                case "credencial":
                    _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
                    break;
                case "dataentrada":
                    if (DateTime.TryParse(val, out DateTime dt))
                    {
                        _ = Query.Where(x => x.DataEntrada.Date == dt.Date);
                    }
                    break;
                case "utenteid":
                    if (Guid.TryParse(val, out Guid utenteId))
                    {
                        _ = Query.Where(x => x.UtenteId == utenteId);
                    }
                    break;
                case "medicoid":
                    if (Guid.TryParse(val, out Guid medicoId))
                    {
                        _ = Query.Where(x => x.MedicoId == medicoId);
                    }
                    break;
                case "prioridadeid":
                    if (Guid.TryParse(val, out Guid prioridadeId))
                    {
                        _ = Query.Where(x => x.PrioridadeId == prioridadeId);
                    }
                    break;
                case "localtratamentoid":
                    if (Guid.TryParse(val, out Guid localId))
                    {
                        _ = Query.Where(x => x.LocalTratamentoId == localId);
                    }
                    break;
                case "estadolistaid":
                    if (Guid.TryParse(val, out Guid estadoId))
                    {
                        _ = Query.Where(x => x.EstadoListaEsperaId == estadoId);
                    }
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(dynamicOrder))
        {
            _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.DataEntrada).ThenBy(x => x.CreatedOn);
        }
        else
        {
            _ = Query.OrderBy(dynamicOrder);
        }
    }
}
