using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Specifications
{
    public class AnamneseOrtodonticaAnaliseDentariaSearchTable : Specification<AnamneseOrtodonticaAnaliseDentaria>
    {
        public AnamneseOrtodonticaAnaliseDentariaSearchTable(Guid? utenteId, string? keyword, string? dynamicOrder = "")
        {

            _ = Query.Include(h => h.Utente);

            // filters
            if (utenteId.HasValue && utenteId.Value != Guid.Empty)
            {
                _ = Query.Where(x => x.UtenteId == utenteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Utente.Nome.Contains(keyword));
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }


        }
    }
}
