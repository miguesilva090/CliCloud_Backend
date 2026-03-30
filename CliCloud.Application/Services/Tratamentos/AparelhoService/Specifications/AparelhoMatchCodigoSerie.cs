using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.Specifications
{
  public class AparelhoMatchCodigoSerie : Specification<Aparelho>
  {
    public AparelhoMatchCodigoSerie(string? codigoSerie, Guid? excludeId = null)
    {
      if (string.IsNullOrWhiteSpace(codigoSerie))
      {
        _ = Query.Where(x => false);
        return;
      }
      _ = Query.Where(x => x.CodigoSerie != null && x.CodigoSerie == codigoSerie);
      if (excludeId.HasValue)
        _ = Query.Where(x => x.Id != excludeId.Value);
    }
  }
}
