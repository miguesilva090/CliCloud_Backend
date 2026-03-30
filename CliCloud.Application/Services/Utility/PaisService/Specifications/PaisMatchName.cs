using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.PaisService.Specifications
{
    public class PaisMatchName : Specification<Pais>
    {
        public PaisMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
