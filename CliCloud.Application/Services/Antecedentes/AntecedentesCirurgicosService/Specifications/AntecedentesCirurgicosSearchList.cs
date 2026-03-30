using Ardalis.Specification;
using CliCloud.Domain.Entities.Antecedentes;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Specifications
{
    public class AntecedentesCirurgicosSearchList : Specification<AntecedentesCirurgicos>
    {
        public AntecedentesCirurgicosSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.TipoCirurgia != null && x.TipoCirurgia.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
