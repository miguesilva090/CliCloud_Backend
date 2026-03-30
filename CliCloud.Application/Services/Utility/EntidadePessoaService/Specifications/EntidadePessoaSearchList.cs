using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService.Specifications
{
    public class EntidadePessoaSearchList : Specification<EntidadePessoa>
    {
        public EntidadePessoaSearchList(string? keyword = "")
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
