using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Specifications
{
    public class AnamneseOrtodonticaAnaliseGeralMatchName : Specification<AnamneseOrtodonticaAnaliseGeral>
    {
        public AnamneseOrtodonticaAnaliseGeralMatchName(string? name)
        {
            _ = Query.Include(h => h.Utente);

            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Utente.Nome.Contains(name));
            }
            _ = Query.OrderBy(h => h.Utente.Nome);
        }
    }
}
