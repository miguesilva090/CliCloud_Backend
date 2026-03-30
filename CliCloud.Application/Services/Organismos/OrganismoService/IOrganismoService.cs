using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.Filters;

namespace CliCloud.Application.Services.Organismos.OrganismoService
{
    public interface IOrganismoService : ITransientService
    {
        Task<Response<IEnumerable<OrganismoDTO>>> GetOrganismoAsync(string keyword = "");
        Task<Response<IEnumerable<OrganismoLightDTO>>> GetOrganismoLightAsync(string keyword = "");
        Task<PaginatedResponse<OrganismoTableDTO>> GetOrganismoPaginatedAsync(OrganismoTableFilter filter);
        Task<Response<IEnumerable<OrganismoTableDTO>>> GetAllOrganismoAsync(OrganismoAllFilter filter);
        Task<Response<OrganismoDTO>> GetOrganismoAsync(Guid id);
        Task<Response<OrganismoDTO>> GetOrganismoByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<OrganismoDTO>>> GetOrganismoByNameAsync(string nome);
        Task<Response<Guid>> CreateOrganismoAsync(CreateOrganismoRequest request);
        Task<Response<Guid>> UpdateOrganismoAsync(UpdateOrganismoRequest request, Guid id);
        Task<Response<Guid>> DeleteOrganismoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleOrganismoAsync(IEnumerable<Guid> ids);
    }
}
