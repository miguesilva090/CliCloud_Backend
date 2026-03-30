using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Specifications
{
  public class MarcacaoConsultaSearchTable : Specification<ConsultaMarcacao>
  {
    public MarcacaoConsultaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
        {
          var id = f.Id?.ToLowerInvariant();
          var val = f.Value;
          if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val)) continue;

          switch (id)
          {
            case "data":
              if (DateTime.TryParse(val, out var dt))
                _ = Query.Where(x => x.Data != null && x.Data.Value.Date == dt.Date);
              break;
            case "horainic":
            case "horafim":
              if (TimeSpan.TryParse(val, out var ts))
                _ = Query.Where(x => x.HoraMarcacao != null && x.HoraMarcacao.Value == ts);
              break;
            case "obs":
              _ = Query.Where(x => x.Obs != null && x.Obs.Contains(val));
              break;
            case "utenteid":
              if (Guid.TryParse(val, out var utId)) _ = Query.Where(x => x.UtenteId == utId);
              break;
            case "medicoid":
              if (Guid.TryParse(val, out var mId))
                _ = Query.Where(x => x.MedicoId == null || x.MedicoId == mId);
              break;
            case "especialidadeid":
              if (Guid.TryParse(val, out var eId)) _ = Query.Where(x => x.EspecialidadeId == eId);
              break;
            case "consultaid":
              if (Guid.TryParse(val, out var cId)) _ = Query.Where(x => x.ConsultaId == cId);
              break;
          }
        }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderByDescending(x => x.Data);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

