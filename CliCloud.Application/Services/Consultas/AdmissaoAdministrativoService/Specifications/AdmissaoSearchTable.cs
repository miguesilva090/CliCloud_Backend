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
    DateTime dataReferencia,
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
      .Include(x => x.TipoAdmissao)
      .Include(x => x.TipoConsultaItem);

    _ = Query.Where(x =>
      x.DeletedOn == null
      && (x.StatusConsulta == null || x.StatusConsulta != StatusConsulta.Desmarcada)
    );

    DateTime refDate = dataReferencia.Date;
    if (modo == ModoListagemAdmissao.ParaFaturacao)
    {
      // Sem filtro por data do documento — alinhado ao legado TfaturaEdt (utente + débitos).
    }
    else if (modo == ModoListagemAdmissao.Pendentes)
    {
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date < refDate);
    }
    else
    {
      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == refDate);
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
        case "confirmaconsulta":
        case "confirma_consulta":
          if (bool.TryParse(val, out bool confirmaConsulta))
          {
            _ = Query.Where(x => x.ConfirmaConsulta == confirmaConsulta);
          }

          break;
        case "emtratamento":
        case "em_tratamento":
          if (bool.TryParse(val, out bool emTratamento))
          {
            _ = Query.Where(x => x.EmTratamento == emTratamento);
          }

          break;
        case "credencial":
          _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
          break;
        case "faturado":
          if (bool.TryParse(val, out bool faturado))
          {
            _ = faturado
              ? Query.Where(x => x.Faturado == true)
              : Query.Where(x => x.Faturado != true);
          }

          break;
        case "pago":
          if (bool.TryParse(val, out bool pago))
          {
            _ = Query.Where(x => x.Pago == pago);
          }

          break;
      }
    }

    if (string.IsNullOrWhiteSpace(dynamicOrder))
    {
      if (modo == ModoListagemAdmissao.ParaFaturacao)
      {
        _ = Query.OrderByDescending(x => x.Data).ThenByDescending(x => x.HoraInicio);
      }
      else
      {
        _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.HoraInicio);
      }
    }
    else
    {
      _ = Query.OrderBy(dynamicOrder);
    }
  }
}
