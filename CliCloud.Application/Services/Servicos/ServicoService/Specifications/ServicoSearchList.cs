using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.ServicoService.Specifications
{
  public class ServicoSearchList : Specification<Servico>
  {
    public ServicoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x =>
          x.Designacao.Contains(keyword)
          || (x.EAN != null && x.EAN.Contains(keyword))
        );

      _ = Query.OrderBy(x => x.Designacao);
    }
  }
}

