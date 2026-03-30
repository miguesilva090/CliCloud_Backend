using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.Specifications
{
  public class TipoServicoSearchList : Specification<TipoServico>
  {
    public TipoServicoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x => x.Descricao.Contains(keyword));
      _ = Query.OrderBy(x => x.Descricao);
    }
  }
}

