using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Specifications;

public sealed class HistoricoConsultaAdministrativoSearchSpec : Specification<Consulta>
{
  public HistoricoConsultaAdministrativoSearchSpec(List<TableFilter> filters, string? dynamicOrder = "")
  {
    _ = Query.Include(x => x.TipoConsultaItem)
      .Include(x => x.Utente)
      .Include(x => x.Organismo)
      .Include(x => x.Sala)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.ConsultaMarcacao)
        .ThenInclude(cm => cm!.TipoAdmissao);

    if (filters is { Count: > 0 })
    {
      foreach (TableFilter f in filters)
      {
        string? id = f.Id?.ToLowerInvariant();
        string? val = f.Value;
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val))
        {
          continue;
        }

        switch (id)
        {
          case "id":
            if (Guid.TryParse(val, out Guid idGuid))
            {
              _ = Query.Where(x => x.Id == idGuid);
            }

            break;
          case "sala":
            _ = Query.Where(x => x.Sala != null && x.Sala.Nome.Contains(val));
            break;
          case "horainic":
            if (TimeSpan.TryParse(val, out TimeSpan tsIni))
            {
              _ = Query.Where(x => x.HoraInicio.HasValue && x.HoraInicio.Value == tsIni);
            }

            break;
          case "horafim":
            if (TimeSpan.TryParse(val, out TimeSpan tsFim))
            {
              _ = Query.Where(x => x.HoraFim.HasValue && x.HoraFim.Value == tsFim);
            }

            break;
          case "diagnostico":
            _ = Query.Where(x => x.Diagnostico != null && x.Diagnostico.Contains(val));
            break;
          case "obs":
            _ = Query.Where(x => x.Obs != null && x.Obs.Contains(val));
            break;
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
            if (Guid.TryParse(val, out Guid uId))
            {
              _ = Query.Where(x => x.UtenteId == uId);
            }

            break;
          case "medicoid":
            if (Guid.TryParse(val, out Guid mId))
            {
              _ = Query.Where(x => x.MedicoId == mId);
            }

            break;
          case "especialidadeid":
            if (Guid.TryParse(val, out Guid eId))
            {
              _ = Query.Where(x => x.EspecialidadeId == eId);
            }

            break;
          case "tecnicoid":
            if (Guid.TryParse(val, out Guid tId))
            {
              _ = Query.Where(x => x.TecnicoId == tId);
            }

            break;
          case "organismoid":
            if (Guid.TryParse(val, out Guid orgId))
            {
              _ = Query.Where(x => x.OrganismoId == orgId);
            }

            break;
          case "organismocodigoclinica":
            _ = Query.Where(x =>
              x.Organismo != null
              && x.Organismo.CodigoClinica != null
              && x.Organismo.CodigoClinica.Contains(val));
            break;
          case "seguradoraid":
            if (Guid.TryParse(val, out Guid segId))
            {
              _ = Query.Where(x => x.SeguradoraId == segId);
            }

            break;
          case "tratamentoid":
            if (Guid.TryParse(val, out Guid trId))
            {
              _ = Query.Where(x => x.TratamentoId == trId);
            }

            break;
          case "funcionarioid":
            if (Guid.TryParse(val, out Guid fId))
            {
              _ = Query.Where(x => x.FuncionarioId == fId);
            }

            break;
          case "tipoconsultaid":
            if (Guid.TryParse(val, out Guid tcId))
            {
              _ = Query.Where(x => x.TipoConsultaId == tcId);
            }

            break;
          case "credencial":
            _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
            break;
          case "filtrobox":
            _ = Query.Where(x =>
              (x.Utente != null && x.Utente.Nome.Contains(val))
              || (x.Utente != null
                  && x.Utente.NumeroUtente != null
                  && x.Utente.NumeroUtente.Contains(val)));
            break;
          case "efectuado":
            if (bool.TryParse(val, out bool ef) && ef)
            {
              _ = Query.Where(x => x.Efetuado == true);
            }
            else if (bool.TryParse(val, out bool efFalse) && !efFalse)
            {
              _ = Query.Where(x => x.Efetuado != true);
            }

            break;
          case "faltou":
            if (bool.TryParse(val, out bool fl) && fl)
            {
              _ = Query.Where(x =>
                x.Faltou == true
                || x.StatusConsulta == StatusConsulta.Faltou
                || x.StatusConsulta == StatusConsulta.FaltouJustificada);
            }

            break;
          case "confirmado":
            if (bool.TryParse(val, out bool cf) && cf)
            {
              _ = Query.Where(x => x.Confirmado == true);
            }
            else if (bool.TryParse(val, out bool cfFalse) && !cfFalse)
            {
              _ = Query.Where(x => x.Confirmado != true);
            }

            break;
        }
      }
    }

    if (string.IsNullOrEmpty(dynamicOrder))
    {
      _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
    }
    else
    {
      _ = Query.OrderBy(NormalizeHistoricoOrder(dynamicOrder));
    }
  }

  private static string NormalizeHistoricoOrder(string orderByFields)
  {
    if (string.IsNullOrWhiteSpace(orderByFields))
    {
      return orderByFields;
    }

    string[] segments = orderByFields.Split(
      ',',
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
    );
    for (int i = 0; i < segments.Length; i++)
    {
      string s = segments[i];
      bool desc = s.StartsWith('-');
      string field = desc ? s[1..] : s;

      string mapped = field.Equals("horaInic", StringComparison.OrdinalIgnoreCase)
        ? nameof(Consulta.HoraInicio)
        : field.Equals("horaFim", StringComparison.OrdinalIgnoreCase)
          ? nameof(Consulta.HoraFim)
          : field;

      segments[i] = desc ? "-" + mapped : mapped;
    }

    return string.Join(",", segments);
  }
}
