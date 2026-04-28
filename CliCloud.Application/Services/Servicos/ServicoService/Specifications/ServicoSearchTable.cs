using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.ServicoService.Specifications
{
  public class ServicoSearchTable : Specification<Servico>
  {
    public ServicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "designacao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Designacao.Contains(f.Value));
              break;
            case "tiposervicoid":
              if (Guid.TryParse(f.Value, out var tsId)) _ = Query.Where(x => x.TipoServicoId == tsId);
              break;
            case "inativo":
              if (bool.TryParse(f.Value, out var b)) _ = Query.Where(x => x.Inativo == b);
              break;
          }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Designacao);
      }
      else
      {
        _ = Query.OrderBy(NormalizeServicoOrder(dynamicOrder));
      }
    }

    private static string NormalizeServicoOrder(string orderByFields)
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

        string mapped = field.Equals("tipoServicoDescricao", StringComparison.OrdinalIgnoreCase)
          ? "TipoServico.Descricao"
          : field;

        segments[i] = desc ? "-" + mapped : mapped;
      }

      return string.Join(",", segments);
    }
  }
}

