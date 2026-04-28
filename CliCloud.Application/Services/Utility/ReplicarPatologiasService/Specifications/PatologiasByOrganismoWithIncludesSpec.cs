using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Utility.ReplicarPatologiasService.Specifications;

public class PatologiasByOrganismoWithIncludesSpec : Specification<Patologia>
{
    public PatologiasByOrganismoWithIncludesSpec(Guid organismoId) 
    {
        Query.Where(x => x.OrganismoId == organismoId)
            .Include(x => x.PatologiaServicos)
            .Include(x => x.PatologiaDoencas);
    }
}
