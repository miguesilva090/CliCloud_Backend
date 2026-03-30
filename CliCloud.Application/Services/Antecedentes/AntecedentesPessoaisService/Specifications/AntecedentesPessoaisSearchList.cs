using Ardalis.Specification;
using CliCloud.Domain.Entities.Antecedentes;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Specifications
{
    public class AntecedentesPessoaisSearchList : Specification<AntecedentesPessoais>
    {
        public AntecedentesPessoaisSearchList(string? keyword = "")
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
