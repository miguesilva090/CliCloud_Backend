using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Filters;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;

public sealed class MarcacaoAdministrativoSearchTable : Specification<ConsultaMarcacao>
{
  public MarcacaoAdministrativoSearchTable(
    MarcacaoAdministrativoTableFilter filter,
    string? dynamicOrder = ""
  )
  {
    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade);

    if(filter.ApenasAtivas)
    {
      _ = Query.Where(x =>
        x.DeletedOn == null
        && (
          x.StatusConsulta == null
          || (
            x.StatusConsulta != StatusConsulta.Desmarcada
            && x.StatusConsulta != StatusConsulta.Suspensa
            && x.StatusConsulta != StatusConsulta.Concluida
          )
        )
      );
    }

    if(filter.DataDe.HasValue)
    {
      DateTime dataDe = filter.DataDe.Value.Date;
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date >= dataDe);
    }

    if(filter.DataAte.HasValue)
    {
      DateTime dataAte = filter.DataAte.Value.Date;
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date <= dataAte);
    }

    if(filter.HoraDe.HasValue)
    {
      _ = Query.Where(x => x.HoraMarcacao.HasValue && x.HoraMarcacao.Value >= filter.HoraDe.Value);
    }

    if(filter.HoraAte.HasValue)
    {
      _ = Query.Where(x => x.HoraMarcacao.HasValue && x.HoraMarcacao.Value <= filter.HoraAte.Value);
    }

    if(filter.MedicoId.HasValue)
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

    foreach (TableFilter f in filter.Filters ?? [])
    {
      string id = (f.Id ?? string.Empty).ToLowerInvariant();
      string? val = f.Value;
      if(string.IsNullOrWhiteSpace(val))
      {
        continue;
      }

      switch(id)
      {
        case "data":
          if(DateTime.TryParse(val, out DateTime dt))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
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
          if(Guid.TryParse(val, out Guid espId))
          {
            _ = Query.Where(x => x.EspecialidadeId == espId);
          }
          break;
      }
    }

    if(string.IsNullOrWhiteSpace(dynamicOrder))
    {
      _ = Query.OrderBy(x => x.Data).ThenBy(x => x.HoraMarcacao);
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}
