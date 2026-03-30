using Ardalis.Specification;
using CliCloud.Domain.Entities.EntidadesFinanceiras;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraMatchNContrib : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}
