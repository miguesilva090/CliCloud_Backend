using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications;

public sealed class FuncionarioByEmailSpec : Specification<Funcionario>
{
  public FuncionarioByEmailSpec(string email)
  {
    string normalized = email.Trim().ToLowerInvariant();
    _ = Query.Where(f => f.Email != null && f.Email.ToLower() == normalized);
    _ = Query.OrderBy(f => f.Nome);
  }
}
