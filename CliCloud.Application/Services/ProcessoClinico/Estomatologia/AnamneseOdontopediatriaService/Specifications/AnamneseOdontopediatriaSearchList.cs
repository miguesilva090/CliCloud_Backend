using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;


namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Specifications
{
    public class AnamneseOdontopediatriaSearchList : Specification<AnamneseOdontopediatria>
    {
        public AnamneseOdontopediatriaSearchList(string? keyword = "")
        {

            _ = Query.Include(h => h.Utente);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Utente.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
