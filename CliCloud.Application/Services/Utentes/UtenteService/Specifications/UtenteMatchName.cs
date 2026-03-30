using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
    public class UtenteMatchName : Specification<Utente>
    {
        public UtenteMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
