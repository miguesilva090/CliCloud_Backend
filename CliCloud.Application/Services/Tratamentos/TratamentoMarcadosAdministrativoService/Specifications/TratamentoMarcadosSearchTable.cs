using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Filters;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Specifications;

public sealed class TratamentoMarcadosSearchTable : Specification<Tratamento>
{
  public TratamentoMarcadosSearchTable(
    TratamentoMarcadosTableFilter filter,
    string? dynamicOrder = ""
  )
  {
    DateTime hoje = DateTime.Today;

    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Organismo)
      .Include(x => x.LocalTratamento);

    _ = Query.Where(x => x.DeletedOn == null);

    if (filter.Modo == ModoListagemTratamentoMarcados.Marcados)
    {
      _ = Query.Where(x => 
        x.DataInic.HasValue
        && (x.DataFim == null || x.DataFim.Value.Date > hoje)
        && (x.Suspenso ?? 0) == 0
        && (x.Provisorio ?? 0) == 0
      );
    }

    if (filter.LocalTratamentoId.HasValue)
    {
      _ = Query.Where(x => x.LocalTratamentoId == filter.LocalTratamentoId.Value);
    }
    else if (filter.Modo == ModoListagemTratamentoMarcados.PorLocal)
    {
      _ = Query.Where(x => false);
    }

    if (filter.UtenteId.HasValue)
    {
      _ = Query.Where(x => x.UtenteId == filter.UtenteId.Value);
    }
    else if (filter.Modo == ModoListagemTratamentoMarcados.PorUtente)
    {
      _ = Query.Where(x => false);
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
        case "utentenome":
          _ = Query.Where(x =>
            x.Utente != null
            && (
              (x.Utente.Nome != null && x.Utente.Nome.Contains(val))
              || (x.Utente.NumeroUtente != null
                && x.Utente.NumeroUtente.Contains(val))
            )
          );
          break;

        case "designacao":
          _ = Query.Where(x =>
            x.Designacao != null && x.Designacao.Contains(val)
          );
          break;

        case "credencial":
          _ = Query.Where(x =>
            x.Credencial != null && x.Credencial.Contains(val)
          );
          break;

        case "numcartao":
          _ = Query.Where(x =>
            x.NumCartao != null && x.NumCartao.Contains(val)
          );
          break;

        case "localtratamentoid":
          if (Guid.TryParse(val, out Guid localId))
          {
            _ = Query.Where(x => x.LocalTratamentoId == localId);
          }
          break;

        case "utenteid":
          if (Guid.TryParse(val, out Guid utenteId))
          {
            _ = Query.Where(x => x.UtenteId == utenteId);
          }
          break;

        case "fisioterapeutaid":
          if (Guid.TryParse(val, out Guid fisioId))
          {
            _ = Query.Where(x => x.FisioterapeutaId == fisioId);
          }
          break;

        case "auxiliarid":
          if (Guid.TryParse(val, out Guid auxId))
          {
            _ = Query.Where(x => x.AuxiliarId == auxId);
          }
          break;

        case "organismoid":
          if (Guid.TryParse(val, out Guid orgId))
          {
            _ = Query.Where(x => x.OrganismoId == orgId);
          }
          break;

        case "datainicde":
          if (DateTime.TryParse(val, out DateTime diDe))
          {
            _ = Query.Where(x => x.DataInic.HasValue && x.DataInic.Value.Date >= diDe.Date);
          }
          break;

        case "datainicate":
          if (DateTime.TryParse(val, out DateTime diAte))
          {
            _ = Query.Where(x =>
              x.DataInic.HasValue && x.DataInic.Value.Date <= diAte.Date
            );
          }
          break;

        case "datafimde":
          if (DateTime.TryParse(val, out DateTime dfDe))
          {
            _ = Query.Where(x =>
              x.DataFim.HasValue && x.DataFim.Value.Date >= dfDe.Date
            );
          }
          break;

        case "datafimate":
          if (DateTime.TryParse(val, out DateTime dfAte))
          {
            _ = Query.Where(x =>
              x.DataFim.HasValue && x.DataFim.Value.Date <= dfAte.Date
            );
          }
          break;

        // Paridade Filtro_Estado (legado TRATAMENLst)
        case "filtroestado":
        case "estado":
          switch (val.Trim().ToLowerInvariant())
          {
            case "por iniciar":
            case "poriniciar":
              _ = Query.Where(x =>
                x.DataInic.HasValue
                && x.DataInic.Value.Date >= hoje
                && (x.Provisorio ?? 0) == 0
                && (x.Suspenso ?? 0) == 0
              );
              break;
            case "iniciados":
              _ = Query.Where(x =>
                x.DataInic.HasValue
                && x.DataInic.Value.Date < hoje
                && (x.DataFim == null || x.DataFim.Value.Date > hoje)
                && (x.Suspenso ?? 0) == 0
                && (x.Provisorio ?? 0) == 0
              );
              break;
            case "terminados":
              _ = Query.Where(x =>
                x.DataFim.HasValue
                && x.DataFim.Value.Date <= hoje
                && (x.Suspenso ?? 0) == 0
                && (x.Provisorio ?? 0) == 0
              );
              break;
            case "suspensos":
              _ = Query.Where(x => x.Suspenso == 1);
              break;
            case "provisorios":
            case "provisórios":
              _ = Query.Where(x => x.Provisorio == 1);
              break;
          }
          break;
      }
    }

    if (string.IsNullOrEmpty(dynamicOrder))
    {
      _ = Query.OrderByDescending(x => x.DataInic ?? x.CreatedOn);
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}