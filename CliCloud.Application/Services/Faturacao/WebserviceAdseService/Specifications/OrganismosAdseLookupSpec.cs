using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.Specifications;

public class OrganismosAdseLookupSpec : Specification<Organismo>
{
    public OrganismosAdseLookupSpec() =>
        Query
            .Where(x =>
                x.DeletedOn == null
                && (x.Status == null || (int)x.Status == 0 || (int)x.Status == 1))
            .OrderBy(x => x.Nome);
}
