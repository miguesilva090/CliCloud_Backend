using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Specifications
{
    public class AnamneseOrtodonticaATMMatchName : Specification<AnamneseOrtodonticaATM>
    {
        public AnamneseOrtodonticaATMMatchName(string? name)
        {
            _ = Query.Include(h => h.Utente);
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Utente.Nome == name);
            }
            _ = Query.OrderBy(h => h.Utente.Nome);
        }
    }
}
