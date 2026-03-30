using Ardalis.Specification;
using CliCloud.Domain.Entities.EntidadesFinanceiras;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications
{
    public class EntidadeFinanceiraSearchList : Specification<EntidadeFinanceira>
    {
        public EntidadeFinanceiraSearchList(string? keyword = "")
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
