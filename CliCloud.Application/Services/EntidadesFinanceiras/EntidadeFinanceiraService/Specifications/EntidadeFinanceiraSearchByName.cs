using Ardalis.Specification;
using CliCloud.Domain.Entities.EntidadesFinanceiras;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraSearchByName : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
        }
    }
}
