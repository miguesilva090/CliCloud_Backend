using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications
{
  public class AtestadoSearchTable : Specification<Atestado>
  {
    public AtestadoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      _ = Query
        .Include(x => x.Utente)
        .Include(x => x.Medico);

      if (filters != null && filters.Count > 0)
      {
        foreach (var f in filters)
        {
          switch (f.Id.ToLowerInvariant())
          {
            case "data":
              if (!string.IsNullOrWhiteSpace(f.Value) && DateTime.TryParse(f.Value, out var data))
                _ = Query.Where(x => x.DataAtestado.Date == data.Date);
              break;
            case "datafim":
              if (!string.IsNullOrWhiteSpace(f.Value) && DateTime.TryParse(f.Value, out var dataFim))
                _ = Query.Where(x => x.DataAtestado.Date <= dataFim.Date);
              break;
            case "idutente":
              if (!string.IsNullOrWhiteSpace(f.Value) && Guid.TryParse(f.Value, out var idUtente))
                _ = Query.Where(x => x.UtenteId == idUtente);
              break;
            case "idmedico":
              if (!string.IsNullOrWhiteSpace(f.Value) && Guid.TryParse(f.Value, out var idMedico))
                _ = Query.Where(x => x.MedicoId == idMedico);
              break;
            case "estadoenvio":
              if (!string.IsNullOrWhiteSpace(f.Value) && int.TryParse(f.Value, out var estadoEnvio))
                _ = Query.Where(x => x.EstadoEnvio == estadoEnvio);
              break;
            case "numerospms":
              if (!string.IsNullOrWhiteSpace(f.Value))
                _ = Query.Where(x => x.NumeroSPMS != null && x.NumeroSPMS.Contains(f.Value));
              break;
            case "numerosns":
              if (!string.IsNullOrWhiteSpace(f.Value))
                _ = Query.Where(x => x.NumeroSNS != null && x.NumeroSNS.Contains(f.Value));
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderByDescending(x => x.DataAtestado).ThenByDescending(x => x.CreatedOn);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
