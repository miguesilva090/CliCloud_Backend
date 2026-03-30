using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.ConcelhoService.Specifications
{
    public class ConcelhoMatchName : Specification<Concelho>
    {
        public ConcelhoMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
