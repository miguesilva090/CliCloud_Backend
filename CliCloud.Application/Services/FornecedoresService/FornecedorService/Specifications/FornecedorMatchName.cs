using Ardalis.Specification;
using CliCloud.Domain.Entities.Fornecedores;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.Specifications
{
    public class FornecedorMatchName : Specification<Fornecedor>
    {
        public FornecedorMatchName(string nome)
        {
            _ = Query.Where(x => x.Nome == nome);
        }
    }
}
