using Ardalis.Specification;
using CliCloud.Domain.Entities.EntidadesFinanceiras;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraMatchName : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraMatchName(string nome)
        {
            _ = Query.Where(x => x.Nome == nome);
        }
    }
}
