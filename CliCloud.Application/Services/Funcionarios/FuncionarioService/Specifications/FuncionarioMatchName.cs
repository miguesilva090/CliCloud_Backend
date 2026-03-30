using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications
{
    public class FuncionarioMatchName : Specification<Funcionario>
    {
        public FuncionarioMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
