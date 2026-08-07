using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Filters;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Specifications;

public sealed class AdmissaoTratamentoSearchTable : Specification<SessaoTratamento>
{
  public AdmissaoTratamentoSearchTable(
    AdmissaoTratamentoTableFilter filter,
    string? dynamicOrder = ""
  )
  {
    _ = Query
      .Include(x => x.Tratamento!)
        .ThenInclude(t => t.Utente)
      .Include(x => x.Tratamento!)
        .ThenInclude(t => t.LocalTratamento)
      .Include(x => x.Fisioterapeuta)
      .Include(x => x.Auxiliar)
      .Include(x => x.OutroTecnico);

    _ = Query.Where(x => x.DeletedOn == null);
    _ = Query.Where(x => x.HistSess == null || x.HistSess == 0);
    if (!filter.IncluirDesmarcados)
    {
      _ = Query.Where(x => x.Desmarcado == null || x.Desmarcado == 0);
    }

    DateTime dia = (filter.DataReferencia ?? DateTime.Today).Date;
    _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dia);

    if (filter.Modo == ModoListagemAdmissaoTratamento.Presentes)
    {
      _ = Query.Where(x => x.Confirmado == 1);
    }

    if (filter.LocalTratamentoId.HasValue)
    {
      _ = Query.Where(x =>
        x.Tratamento != null
        && x.Tratamento.LocalTratamentoId == filter.LocalTratamentoId.Value
      );
    }
    else if (filter.Modo == ModoListagemAdmissaoTratamento.LocalTratamento)
    {
      _ = Query.Where(x => false);
    }

    if (filter.FisioterapeutaId.HasValue)
    {
      _ = Query.Where(x => x.FisioterapeutaId == filter.FisioterapeutaId.Value);
    }

    if (filter.UtenteId.HasValue)
    {
      _ = Query.Where(x =>
        x.Tratamento != null && x.Tratamento.UtenteId == filter.UtenteId.Value
      );
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
            x.Tratamento != null
            && x.Tratamento.Utente != null
            && (
              (x.Tratamento.Utente.Nome != null
                && x.Tratamento.Utente.Nome.Contains(val))
              || (x.Tratamento.Utente.NumeroUtente != null
                && x.Tratamento.Utente.NumeroUtente.Contains(val))
            )
          );
          break;
        case "data":
        case "datareferencia":
          if (DateTime.TryParse(val, out DateTime dt))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
          }
          break;
        case "localtratamentoid":
          if (Guid.TryParse(val, out Guid localId))
          {
            _ = Query.Where(x =>
              x.Tratamento != null && x.Tratamento.LocalTratamentoId == localId
            );
          }
          break;
        case "fisioterapeutaid":
          if (Guid.TryParse(val, out Guid fisioId))
          {
            _ = Query.Where(x => x.FisioterapeutaId == fisioId);
          }
          break;
        case "utenteid":
          if (Guid.TryParse(val, out Guid utenteId))
          {
            _ = Query.Where(x =>
              x.Tratamento != null && x.Tratamento.UtenteId == utenteId
            );
          }
          break;
      }
    }

    if (string.IsNullOrEmpty(dynamicOrder))
    {
      _ = Query
        .OrderBy(x => x.HoraInic)
        .ThenBy(x => x.Tratamento!.Utente!.Nome);
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}