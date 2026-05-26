using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Filters;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Specifications;

public sealed class OrdemEntradaSearchTable : Specification<Admissao>
{
  public OrdemEntradaSearchTable(OrdemEntradaTableFilter filter, string? dynamicOrder = "")
  {
    DateTime dataDe = (filter.DataDe ?? DateTime.UtcNow).Date;
    DateTime dataAte = (filter.DataAte ?? filter.DataDe ?? DateTime.UtcNow).Date;
    if (dataAte < dataDe)
    {
      (dataDe, dataAte) = (dataAte, dataDe);
    }

    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.TipoConsultaItem)
      .Include(x => x.Consulta);

    _ = Query.Where(x => x.DeletedOn == null);
    _ = Query.Where(x => x.Origem == OrigemAdmissao.Marcacao);
    _ = Query.Where(x =>
      x.Data.HasValue && x.Data.Value.Date >= dataDe && x.Data.Value.Date <= dataAte
    );

    if (filter.IncluirHistorico)
    {
      _ = Query.Where(x => x.StatusConsulta == StatusConsulta.Desmarcada);
    }
    else
    {
      _ = Query.Where(x =>
        x.StatusConsulta == null || x.StatusConsulta != StatusConsulta.Desmarcada
      );
    }

    if (filter.UtenteId.HasValue)
    {
      _ = Query.Where(x => x.UtenteId == filter.UtenteId.Value);
    }

    if (filter.MedicoId.HasValue)
    {
      _ = Query.Where(x => x.MedicoId == filter.MedicoId.Value);
    }

    if (filter.EspecialidadeId.HasValue)
    {
      _ = Query.Where(x => x.EspecialidadeId == filter.EspecialidadeId.Value);
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
        case "data":
          if (DateTime.TryParse(val, out DateTime dt))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
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
        case "especialidadeid":
          if (Guid.TryParse(val, out Guid espId))
          {
            _ = Query.Where(x => x.EspecialidadeId == espId);
          }
          break;
        case "confirmado":
          if (bool.TryParse(val, out bool confirmado))
          {
            _ = Query.Where(x => x.Confirmado == confirmado);
          }
          break;
      }
    }

    if (string.IsNullOrWhiteSpace(dynamicOrder))
    {
      _ = Query.OrderBy(x => x.Data).ThenBy(x => x.HoraInicio);
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}
