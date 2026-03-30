using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.ExameService.Specifications
{
  public class ExameSearchTable : Specification<Exame>
  {
    public ExameSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "numeroprescricao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.NumeroPrescricao != null && x.NumeroPrescricao.Contains(f.Value));
              break;
            case "observacoes":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Observacoes != null && x.Observacoes.Contains(f.Value));
              break;
            case "utenteid":
              if (Guid.TryParse(f.Value, out var utenteId)) _ = Query.Where(x => x.UtenteId == utenteId);
              break;
            case "dataprescricao":
              if (DateTime.TryParse(f.Value, out var data)) _ = Query.Where(x => x.DataPrescricao.Date == data.Date);
              break;
          }

      if (string.IsNullOrEmpty(dynamicOrder))
        _ = Query.OrderByDescending(x => x.DataPrescricao);
      else
        _ = Query.OrderBy(dynamicOrder);
    }
  }
}
