using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.Specifications
{
  public class ServicoConsultaSearchTable : Specification<ServicoConsulta>
  {
    public ServicoConsultaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
        {
          var id = f.Id?.ToLowerInvariant();
          var val = f.Value;
          if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val)) continue;

          switch (id)
          {
            case "consultaid":
              if (Guid.TryParse(val, out var cId)) _ = Query.Where(x => x.ConsultaId == cId);
              break;
            case "servicoid":
              if (Guid.TryParse(val, out var sId)) _ = Query.Where(x => x.ServicoId == sId);
              break;
            case "exameid":
              if (Guid.TryParse(val, out var eId)) _ = Query.Where(x => x.ExameId == eId);
              break;
            case "linha":
              if (int.TryParse(val, out var l)) _ = Query.Where(x => x.Linha == l);
              break;
            case "ordem":
              if (int.TryParse(val, out var o)) _ = Query.Where(x => x.Ordem == o);
              break;
            case "electrocardiograma":
              if (int.TryParse(val, out var ecg)) _ = Query.Where(x => x.Electrocardiograma == ecg);
              break;
            case "codigoartigo":
              _ = Query.Where(x => x.CodigoArtigo != null && x.CodigoArtigo.Contains(val));
              break;
            case "nomeartigo":
              _ = Query.Where(x => x.NomeArtigo != null && x.NomeArtigo.Contains(val));
              break;
            case "dente":
              _ = Query.Where(x => x.Dente != null && x.Dente.Contains(val));
              break;
            case "ncheque":
              _ = Query.Where(x => x.NCheque != null && x.NCheque.Contains(val));
              break;
            case "valorut":
              if (decimal.TryParse(val, out var vut)) _ = Query.Where(x => x.ValorUt == vut);
              break;
            case "valorservico":
              if (decimal.TryParse(val, out var vs)) _ = Query.Where(x => x.ValorServico == vs);
              break;
            case "valorartigo":
              if (decimal.TryParse(val, out var va)) _ = Query.Where(x => x.ValorArtigo == va);
              break;
            case "quantidade":
              if (decimal.TryParse(val, out var q)) _ = Query.Where(x => x.Quantidade == q);
              break;
          }
        }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Linha);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

