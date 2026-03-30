using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.Specifications
{
  public class AparelhoByIdWithIncludes : Specification<Aparelho>
  {
    public AparelhoByIdWithIncludes(Guid id)
    {
      _ = Query
        .Where(x => x.Id == id)
        .Include(x => x.TipoAparelho)
        .Include(x => x.ModeloAparelho)
          .ThenInclude(m => m!.MarcaAparelho);
    }
  }
}
