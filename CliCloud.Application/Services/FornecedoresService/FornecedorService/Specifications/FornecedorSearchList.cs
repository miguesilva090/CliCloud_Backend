using Ardalis.Specification;
using CliCloud.Domain.Entities.Fornecedores;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.Specifications
{
    public class FornecedorSearchList : Specification<Fornecedor>
    {
        public FornecedorSearchList(string? keyword = "")
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
