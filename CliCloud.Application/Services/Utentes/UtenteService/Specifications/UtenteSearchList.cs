using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
    public class UtenteSearchList : Specification<Utente>
    {
        public UtenteSearchList(string? keyword = "")
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
