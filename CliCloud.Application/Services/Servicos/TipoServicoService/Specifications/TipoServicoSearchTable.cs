using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.Specifications
{
  public class TipoServicoSearchTable : Specification<TipoServico>
  {
    public TipoServicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
          switch (f.Id.ToLowerInvariant())
          {
            case "descricao":
              if (!string.IsNullOrWhiteSpace(f.Value)) _ = Query.Where(x => x.Descricao.Contains(f.Value));
              break;
            case "partilhasemrequisicao":
              if (bool.TryParse(f.Value, out var b)) _ = Query.Where(x => x.PartilhaSemRequisicao == b);
              break;
          }

      // sort order
      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.Descricao);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

