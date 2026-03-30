using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.Specifications
{
  public class TipoServicoMatchNome : Specification<TipoServico>
  {
    public TipoServicoMatchNome(string? descricao)
    {
      if (!string.IsNullOrWhiteSpace(descricao))
      {
        _ = Query.Where(x => x.Descricao == descricao);
      }
      _ = Query.OrderBy(x => x.Descricao);
    }
  }
}

