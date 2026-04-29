using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Utility.ReplicarSubsistemasService.Specifications;

public class SubsistemasByOrganismoSpec : Specification<SubsistemaServico>
{
    public SubsistemasByOrganismoSpec(Guid organismoId)
    {
        Query.Where(x => x.OrganismoId == organismoId);
    }
}
