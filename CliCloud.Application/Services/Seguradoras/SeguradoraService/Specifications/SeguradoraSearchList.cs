using Ardalis.Specification;
using CliCloud.Domain.Entities.Seguradoras;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.Specifications
{
  public class SeguradoraSearchList : Specification<Seguradora>
  {
    public SeguradoraSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x =>
          x.Nome.Contains(keyword) ||
          (x.Apolice != null && x.Apolice.Contains(keyword)) ||
          (x.Abreviatura != null && x.Abreviatura.Contains(keyword)));
      _ = Query.OrderBy(x => x.Nome);
    }
  }
}
