using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissaoSearchTable : Specification<Admissao>
{
  public AdmissaoSearchTable(
    ModoListagemAdmissao modo,
    List<TableFilter> filters,
    string? dynamicOrder = ""
  )
  {
    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.Organismo)
      .Include(x => x.Sala)
      .Include(x => x.TipoAdmissao);

    DateTime hoje = DateTime.Today;
    if (modo == ModoListagemAdmissao.Pendentes)
    {
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date < hoje);
    }
    else
    {
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == hoje);
    }

    foreach (TableFilter filter in filters ?? [])
    {
      string id = (filter.Id ?? string.Empty).ToLowerInvariant();
      string? val = filter.Value;
      if (string.IsNullOrWhiteSpace(val))
      {
        continue;
      }

      switch (id)
      {
        case "data":
          if (DateTime.TryParse(val, out DateTime dt))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
          }

          break;
        case "data_de":
          if (DateTime.TryParse(val, out DateTime dataDe))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date >= dataDe.Date);
          }

          break;
        case "data_ate":
          if (DateTime.TryParse(val, out DateTime dataAte))
          {
            _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date <= dataAte.Date);
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
        case "confirmado":
          if (bool.TryParse(val, out bool confirmado))
          {
            _ = Query.Where(x => x.Confirmado == confirmado);
          }

          break;
        case "efetuado":
          if (bool.TryParse(val, out bool efetuado))
          {
            _ = Query.Where(x => x.Efetuado == efetuado);
          }

          break;
        case "credencial":
          _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
          break;
      }
    }

    if (string.IsNullOrWhiteSpace(dynamicOrder))
    {
      _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.HoraInicio);
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}
