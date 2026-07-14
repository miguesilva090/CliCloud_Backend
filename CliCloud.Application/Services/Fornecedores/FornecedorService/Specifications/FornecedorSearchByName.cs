using Ardalis.Specification;
using CliCloud.Domain.Entities.Fornecedores;

namespace CliCloud.Application.Services.Fornecedores.FornecedorService.Specifications
{
    public class FornecedorSearchByName : Specification<Fornecedor>
    {
        public FornecedorSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
        }
    }
}
