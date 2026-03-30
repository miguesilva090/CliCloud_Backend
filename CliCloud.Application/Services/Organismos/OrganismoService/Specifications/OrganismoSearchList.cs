using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Organismos.OrganismoService.Specifications
{
    public class OrganismoSearchList : Specification<Organismo>
    {
        public OrganismoSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
