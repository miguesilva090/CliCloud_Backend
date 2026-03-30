using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Organismos.OrganismoService.Specifications
{
    public class OrganismoMatchName : Specification<Organismo>
    {
        public OrganismoMatchName(string nome)
        {
            _ = Query.Where(x => x.Nome == nome);
        }
    }
}
