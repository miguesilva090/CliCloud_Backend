using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.DistritoService.Specifications
{
    public class DistritoMatchName : Specification<Distrito>
    {
        public DistritoMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
