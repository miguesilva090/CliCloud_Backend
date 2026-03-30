using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications
{
  public class FuncionarioSearchByName : Specification<Funcionario>
  {
    public FuncionarioSearchByName(string? name)
    {
      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.Freguesia)
        .ThenInclude(x => x.Concelho)
        .ThenInclude(x => x.Distrito);

        _ = Query.Include(x => x.Rua)
          .ThenInclude(x => x.CodigoPostal);

        _ = Query.Include(x => x.EntidadeContactos);

        if(!string.IsNullOrWhiteSpace(name))
        {
          _ = Query.Where(x => x.Nome.Contains(name));
        }

        _ = Query.OrderBy(x => x.Nome);
    }
  }
}
