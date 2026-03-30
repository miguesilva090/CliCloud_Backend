using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Specifications
{
  public class ServicoTratamentoSearchTable : Specification<ServicoTratamento>
  {
    public ServicoTratamentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
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
            case "tratamentoid":
              if (Guid.TryParse(val, out var tId)) _ = Query.Where(x => x.TratamentoId == tId);
              break;
            case "servicoid":
              if (Guid.TryParse(val, out var sId)) _ = Query.Where(x => x.ServicoId == sId);
              break;
            case "sessaotratamentoid":
              if (Guid.TryParse(val, out var stId)) _ = Query.Where(x => x.SessaoTratamentoId == stId);
              break;
            case "ordem":
              if (int.TryParse(val, out var o)) _ = Query.Where(x => x.Ordem == o);
              break;
            case "usafisioter":
              if (int.TryParse(val, out var uf)) _ = Query.Where(x => x.UsaFisioter == uf);
              break;
            case "usaauxiliar":
              if (int.TryParse(val, out var ua)) _ = Query.Where(x => x.UsaAuxiliar == ua);
              break;
            case "usaoutro":
              if (int.TryParse(val, out var uo)) _ = Query.Where(x => x.UsaOutro == uo);
              break;
            case "preco":
              if (decimal.TryParse(val, out var preco)) _ = Query.Where(x => x.Preco == preco);
              break;
            case "valorut":
              if (decimal.TryParse(val, out var vut)) _ = Query.Where(x => x.ValorUt == vut);
              break;
            case "descinst":
              if (decimal.TryParse(val, out var di)) _ = Query.Where(x => x.DescInst == di);
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

