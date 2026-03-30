using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.Specifications
{
  public class AparelhoSearchList : Specification<Aparelho>
  {
    public AparelhoSearchList(string? keyword = "")
    {
      _ = Query.Include(x => x.TipoAparelho);
      _ = Query.Include(x => x.ModeloAparelho).ThenInclude(m => m!.MarcaAparelho);
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x =>
          (x.CodigoSerie != null && x.CodigoSerie.Contains(keyword)) ||
          (x.CodigoInventario != null && x.CodigoInventario.Contains(keyword)) ||
          (x.Local != null && x.Local.Contains(keyword)) ||
          (x.TipoAparelho != null && x.TipoAparelho.Designacao.Contains(keyword)));
      _ = Query.OrderBy(x => x.CodigoSerie ?? "");
    }
  }
}
