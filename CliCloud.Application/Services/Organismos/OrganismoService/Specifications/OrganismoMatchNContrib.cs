using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Organismos.OrganismoService.Specifications
{
    public class OrganismoMatchNContrib : Specification<Organismo>
    {
        public OrganismoMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}
