using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications
{
  public class FuncionarioMatchNContrib : Specification<Funcionario>
  {
    public FuncionarioMatchNContrib(string? ncontrib)
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
