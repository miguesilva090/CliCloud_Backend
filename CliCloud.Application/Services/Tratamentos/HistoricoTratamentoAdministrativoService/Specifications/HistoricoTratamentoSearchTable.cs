using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Specifications;

public sealed class HistoricoTratamentoSearchTable : Specification<Tratamento>
{
  public HistoricoTratamentoSearchTable(
    HistoricoTratamentoTableFilter filter,
    string? dynamicOrder = ""
  )
  {
    DateTime hoje = DateTime.Today;
    string modo = HistoricoTratamentoAdministrativoModos.Normalize(filter.Modo);

    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Organismo)
      .Include(x => x.Medico)
      .Include(x => x.Fisioterapeuta)
      .Include(x => x.Auxiliar)
      .Include(x => x.OutroTecnico);

    _ = Query.Where(x => x.DeletedOn == null);
    // Concluídos = histórico (paridade funcional vs Marcados activos)
    _ = Query.Where(x =>
      x.DataFim.HasValue && x.DataFim.Value.Date <= hoje
    );

    switch (modo)
    {
      case HistoricoTratamentoAdministrativoModos.Utentes:
        if (!filter.UtenteId.HasValue)
          _ = Query.Where(_ => false);
        else
          _ = Query.Where(x => x.UtenteId == filter.UtenteId.Value);
        break;
      case HistoricoTratamentoAdministrativoModos.Fisioterapeuta:
        if (!filter.FisioterapeutaId.HasValue)
          _ = Query.Where(_ => false);
        else
          _ = Query.Where(x => x.FisioterapeutaId == filter.FisioterapeutaId.Value);
        break;
      case HistoricoTratamentoAdministrativoModos.Auxiliar:
        if (!filter.AuxiliarId.HasValue)
          _ = Query.Where(_ => false);
        else
          _ = Query.Where(x => x.AuxiliarId == filter.AuxiliarId.Value);
        break;
      case HistoricoTratamentoAdministrativoModos.Outro:
        if (!filter.OutroTecnicoId.HasValue)
          _ = Query.Where(_ => false);
        else
          _ = Query.Where(x => x.OutroTecnicoId == filter.OutroTecnicoId.Value);
        break;
      case HistoricoTratamentoAdministrativoModos.Organismo:
        if (!filter.OrganismoId.HasValue)
          _ = Query.Where(_ => false);
        else
          _ = Query.Where(x => x.OrganismoId == filter.OrganismoId.Value);
        break;
      case HistoricoTratamentoAdministrativoModos.Credencial:
        break;
    }

    bool temCredencial = false;

    foreach (TableFilter f in filter.Filters ?? [])
    {
      string id = (f.Id ?? string.Empty).ToLowerInvariant();
      string? val = f.Value;
      if (string.IsNullOrWhiteSpace(val)) continue;

      switch (id)
      {
        case "filtrobox":
        case "utentenome":
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
          temCredencial = true;
          _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
          break;
        case "datafimde":
          if (DateTime.TryParse(val, out DateTime dfDe))
            _ = Query.Where(x => x.DataFim!.Value.Date >= dfDe.Date);
          break;
        case "datafimate":
          if (DateTime.TryParse(val, out DateTime dfAte))
            _ = Query.Where(x => x.DataFim!.Value.Date <= dfAte.Date);
          break;
        case "datainicde":
          if (DateTime.TryParse(val, out DateTime diDe))
            _ = Query.Where(x => x.DataInic.HasValue && x.DataInic.Value.Date >= diDe.Date);
          break;
        case "datainicate":
          if (DateTime.TryParse(val, out DateTime diAte))
            _ = Query.Where(x => x.DataInic.HasValue && x.DataInic.Value.Date <= diAte.Date);
          break;
        case "utenteid":
          if (Guid.TryParse(val, out Guid uid))
            _ = Query.Where(x => x.UtenteId == uid);
          break;
        case "fisioterapeutaid":
          if (Guid.TryParse(val, out Guid fid))
            _ = Query.Where(x => x.FisioterapeutaId == fid);
          break;
        case "auxiliarid":
          if (Guid.TryParse(val, out Guid aid))
            _ = Query.Where(x => x.AuxiliarId == aid);
          break;
        case "outrotecnicoid":
          if (Guid.TryParse(val, out Guid oid))
            _ = Query.Where(x => x.OutroTecnicoId == oid);
          break;
        case "organismoid":
          if (Guid.TryParse(val, out Guid orgId))
            _ = Query.Where(x => x.OrganismoId == orgId);
          break;
      }
    }

    if (modo == HistoricoTratamentoAdministrativoModos.Credencial && !temCredencial)
      _ = Query.Where(_ => false);

    if (!string.IsNullOrWhiteSpace(dynamicOrder))
      _ = Query.OrderBy(dynamicOrder);
    else
      _ = Query.OrderByDescending(x => x.DataFim).ThenBy(x => x.Utente!.Nome);
  }
}
