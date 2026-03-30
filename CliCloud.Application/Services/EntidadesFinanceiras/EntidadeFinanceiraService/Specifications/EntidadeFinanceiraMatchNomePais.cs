using Ardalis.Specification;
using CliCloud.Domain.Entities.EntidadesFinanceiras;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraMatchNomePais : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraMatchNomePais(string nome, string paisPrefixo)
        {
            _ = Query.Where(x => x.Nome == nome && x.PaisPrefixo == paisPrefixo);
        }
    }
}
