using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService.Specifications
{
  public class EntidadePessoaMatchNContrib : Specification<EntidadePessoa>
  {
    public EntidadePessoaMatchNContrib(string? ncontrib)
    {
      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.Freguesia)
        .ThenInclude(x => x.Concelho)
        .ThenInclude(x => x.Distrito);

      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.CodigoPostal);

      _ = Query.Include(x => x.EntidadeContactos);

      if(!string.IsNullOrWhiteSpace(ncontrib))
      {
        _ = Query.Where(x => x.NumeroContribuinte == ncontrib);
      }
    }
  }
}
