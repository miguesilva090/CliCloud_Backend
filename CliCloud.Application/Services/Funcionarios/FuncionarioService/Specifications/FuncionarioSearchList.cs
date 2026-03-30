using Ardalis.Specification;
using CliCloud.Domain.Entities.Funcionarios;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications
{
    public class FuncionarioSearchList : Specification<Funcionario>
    {
        public FuncionarioSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
