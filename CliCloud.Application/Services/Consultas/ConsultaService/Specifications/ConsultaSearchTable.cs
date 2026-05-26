using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications
{
  public class ConsultaSearchTable : Specification<Consulta>
  {
    public ConsultaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      _ = Query.Include(x => x.TipoConsultaItem)
        .Include(x => x.MotivoConsulta)
        .Include(x => x.Utente)
        .Include(x => x.Organismo)
        .Include(x => x.Sala)
        .Include(x => x.Medico)
        .Include(x => x.Especialidade)
        .Include(x => x.ConsultaMarcacao)
          .ThenInclude(cm => cm!.TipoAdmissao);

      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
        {
          var id = f.Id?.ToLowerInvariant();
          var val = f.Value;
          if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val)) continue;

          switch (id)
          {
            case "id":
              if (Guid.TryParse(val, out var idGuid))
                _ = Query.Where(x => x.Id == idGuid);
              break;
            case "sala":
              _ = Query.Where(x => x.Sala != null && x.Sala.Nome.Contains(val));
              break;
            case "horainic":
              if (TimeSpan.TryParse(val, out var tsIni))
                _ = Query.Where(x => x.HoraInicio.HasValue && x.HoraInicio.Value == tsIni);
              break;
            case "horafim":
              if (TimeSpan.TryParse(val, out var tsFim))
                _ = Query.Where(x => x.HoraFim.HasValue && x.HoraFim.Value == tsFim);
              break;
            case "diagnostico":
              _ = Query.Where(x => x.Diagnostico != null && x.Diagnostico.Contains(val));
              break;
            case "obs":
              _ = Query.Where(x => x.Obs != null && x.Obs.Contains(val));
              break;
            case "data":
              if (DateTime.TryParse(val, out var dt))
                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
              break;
            case "data_de":
              if (DateTime.TryParse(val, out var dataDe))
                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date >= dataDe.Date);
              break;
            case "data_ate":
              if (DateTime.TryParse(val, out var dataAte))
                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date <= dataAte.Date);
              break;
            case "utenteid":
              if (Guid.TryParse(val, out var uId))
                _ = Query.Where(x => x.UtenteId == uId);
              break;
            case "utente_numero_de":
            case "c_utente_de":
              if (int.TryParse(val, out var utenteNumeroDe))
                _ = Query.Where(x =>
                  x.Utente != null
                  && x.Utente.NumeroUtente != null
                  && Convert.ToInt32(x.Utente.NumeroUtente) >= utenteNumeroDe);
              break;
            case "utente_numero_ate":
            case "c_utente_ate":
              if (int.TryParse(val, out var utenteNumeroAte))
                _ = Query.Where(x =>
                  x.Utente != null
                  && x.Utente.NumeroUtente != null
                  && Convert.ToInt32(x.Utente.NumeroUtente) <= utenteNumeroAte);
              break;
            case "medicoid":
              if (Guid.TryParse(val, out var mId))
                _ = Query.Where(x => x.MedicoId == mId);
              break;
            case "especialidadeid":
              if (Guid.TryParse(val, out var eId))
                _ = Query.Where(x => x.EspecialidadeId == eId);
              break;
            case "tecnicoid":
              if (Guid.TryParse(val, out var tId))
                _ = Query.Where(x => x.TecnicoId == tId);
              break;
            case "organismoid":
              if (Guid.TryParse(val, out var orgId))
                _ = Query.Where(x => x.OrganismoId == orgId);
              break;
            case "seguradoraid":
              if (Guid.TryParse(val, out var segId))
                _ = Query.Where(x => x.SeguradoraId == segId);
              break;
            case "tratamentoid":
              if (Guid.TryParse(val, out var trId))
                _ = Query.Where(x => x.TratamentoId == trId);
              break;
            case "funcionarioid":
              if (Guid.TryParse(val, out var fId))
                _ = Query.Where(x => x.FuncionarioId == fId);
              break;
            case "tipoconsultaid":
              if (Guid.TryParse(val, out var tcId))
                _ = Query.Where(x => x.TipoConsultaId == tcId);
              break;
            case "credencial":
              _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
              break;
            case "efectuado":
            case "efetuado":
              if (bool.TryParse(val, out var efetuado))
                _ = Query.Where(x => x.Efetuado == efetuado);
              break;
          }
        }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
      }
      else
      {
        _ = Query.OrderBy(NormalizeConsultaOrder(dynamicOrder));
      }
    }

    /// <summary>
    /// Os ids de ordenação vindos do frontend (camelCase / DTO) nem sempre coincidem com as propriedades da entidade (ex.: horaInic → HoraInicio).
    /// </summary>
    private static string NormalizeConsultaOrder(string orderByFields)
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

        string mapped = field.Equals("data", StringComparison.OrdinalIgnoreCase)
          ? nameof(Consulta.Data)
          : field.Equals("horaInic", StringComparison.OrdinalIgnoreCase)
            ? nameof(Consulta.HoraInicio)
            : field.Equals("horaFim", StringComparison.OrdinalIgnoreCase)
              ? nameof(Consulta.HoraFim)
              : field;

        segments[i] = desc ? "-" + mapped : mapped;
      }

      return string.Join(",", segments);
    }
  }
}
