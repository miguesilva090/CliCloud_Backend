using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Specifications
{
  public class ServicoSessaoSearchTable : Specification<ServicoSessao>
  {
    public ServicoSessaoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
        {
          var id = f.Id?.ToLowerInvariant();
          var val = f.Value;
          if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val)) continue;

          switch (id)
          {
            case "id":
              if (Guid.TryParse(val, out var idGuid)) _ = Query.Where(x => x.Id == idGuid);
              break;
            case "sessaotratamentoid":
              if (Guid.TryParse(val, out var sId)) _ = Query.Where(x => x.SessaoTratamentoId == sId);
              break;
            case "fisioterapeutaid":
              if (Guid.TryParse(val, out var fId)) _ = Query.Where(x => x.FisioterapeutaId == fId);
              break;
            case "auxiliarid":
              if (Guid.TryParse(val, out var aId)) _ = Query.Where(x => x.AuxiliarId == aId);
              break;
            case "servicoid":
              if (Guid.TryParse(val, out var svId)) _ = Query.Where(x => x.ServicoId == svId);
              break;
            case "aparelhoid":
              if (Guid.TryParse(val, out var apId)) _ = Query.Where(x => x.AparelhoId == apId);
              break;
            case "horainic":
              _ = Query.Where(x => x.HoraInic != null && x.HoraInic.Contains(val));
              break;
            case "horafim":
              _ = Query.Where(x => x.HoraFim != null && x.HoraFim.Contains(val));
              break;
            case "duracao":
              _ = Query.Where(x => x.Duracao != null && x.Duracao.Contains(val));
              break;
            case "ordem":
              if (int.TryParse(val, out var o)) _ = Query.Where(x => x.Ordem == o);
              break;
            case "preco":
              if (decimal.TryParse(val, out var preco)) _ = Query.Where(x => x.Preco == preco);
              break;
            case "valorut":
              if (decimal.TryParse(val, out var vut)) _ = Query.Where(x => x.ValorUt == vut);
              break;
            case "obs":
              _ = Query.Where(x => x.Obs != null && x.Obs.Contains(val));
              break;
          }
        }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Ordem ?? 0).ThenByDescending(x => x.CreatedOn);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

