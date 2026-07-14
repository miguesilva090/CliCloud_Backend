using Ardalis.Specification;
using CliCloud.Domain.Entities.Fornecedores;

namespace CliCloud.Application.Services.Fornecedores.FornecedorService.Specifications
{
    public class FornecedorMatchNContrib : Specification<Fornecedor>
    {
        public FornecedorMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}
