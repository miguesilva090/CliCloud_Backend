using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications
{
  public class FuncionarioByIdWithIncludes : Specification<Funcionario>
  {
    public FuncionarioByIdWithIncludes(Guid id)
    {
      _ = Query
        .Include(x => x.Rua)
        .Include(x => x.CodigoPostal)
        .Include(x => x.EntidadeContactos)
        .Where(x => x.Id == id);
    }
  }
}
