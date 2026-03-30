using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;


namespace CliCloud.Application.Services.AlergiaUtenteService.Specifications
{
    public class AlergiaUtenteSearchList : Specification<AlergiaUtente>
    {
        public AlergiaUtenteSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => (x.Observacoes != null && x.Observacoes.Contains(keyword)));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
