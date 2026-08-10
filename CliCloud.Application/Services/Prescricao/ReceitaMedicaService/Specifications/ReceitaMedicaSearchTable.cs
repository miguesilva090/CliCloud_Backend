using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Specifications
{
  public class ReceitaMedicaSearchTable : Specification<ReceitaMedica>
  {
    public ReceitaMedicaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      _ = Query.Include(x => x.Utente).Include(x => x.Medico);

      if (filters != null && filters.Count > 0)
      {
        foreach (var f in filters)
        {
          switch (f.Id.ToLowerInvariant())
          {
            case "utenteid":
              if (Guid.TryParse(f.Value, out var utenteId))
                _ = Query.Where(x => x.UtenteId == utenteId);
              break;
            case "medicoid":
              if (Guid.TryParse(f.Value, out var medicoId))
                _ = Query.Where(x => x.MedicoId == medicoId);
              break;
            case "desmaterializada":
              if (int.TryParse(f.Value, out var desmaterializada))
                _ = Query.Where(x => x.Desmaterializada == desmaterializada);
              break;
            case "anulada":
              if (int.TryParse(f.Value, out var anulada))
                _ = Query.Where(x => x.Anulada == anulada);
              break;
            case "enviada":
              if (int.TryParse(f.Value, out var enviada))
                _ = Query.Where(x => x.Enviada == enviada);
              break;
            case "dataprescricao":
              if (DateTime.TryParse(f.Value, out var data))
                _ = Query.Where(x => x.DataPrescricao.Date == data.Date);
              break;
            case "numeroreceita":
              if (!string.IsNullOrWhiteSpace(f.Value))
                _ = Query.Where(x =>
                  (x.NumeroReceita != null && x.NumeroReceita.Contains(f.Value)) ||
                  (x.NumeroReceitaLocal != null && x.NumeroReceitaLocal.Contains(f.Value)));
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderByDescending(x => x.DataPrescricao);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
