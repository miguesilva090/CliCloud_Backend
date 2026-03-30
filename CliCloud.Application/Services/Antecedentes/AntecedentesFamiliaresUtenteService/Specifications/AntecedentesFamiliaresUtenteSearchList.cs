using Ardalis.Specification;
using CliCloud.Domain.Entities.Antecedentes;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Specifications
{
    public class AntecedentesFamiliaresUtenteSearchList : Specification<AntecedentesFamiliaresUtente>
    {
        public AntecedentesFamiliaresUtenteSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.NomeDoenca.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
