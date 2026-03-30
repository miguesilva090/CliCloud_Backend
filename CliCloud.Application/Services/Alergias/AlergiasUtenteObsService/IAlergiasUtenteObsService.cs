using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.AlergiasUtenteObsService.DTOs;
using CliCloud.Application.Services.AlergiasUtenteObsService.Filters;

namespace CliCloud.Application.Services.AlergiasUtenteObsService
{
    public interface IAlergiasUtenteObsService : ITransientService
    {
        Task<Response<IEnumerable<AlergiasUtenteObsDTO>>> GetAlergiasUtenteObsAsync(string keyword = "");
        Task<PaginatedResponse<AlergiasUtenteObsDTO>> GetAlergiasUtenteObsPaginatedAsync(AlergiasUtenteObsTableFilter filter);
        Task<Response<AlergiasUtenteObsDTO>> GetAlergiasUtenteObsAsync(Guid id);
        Task<Response<AlergiasUtenteObsDTO?>> GetByUtenteIdAsync(Guid utenteId);
        Task<Response<Guid>> CreateAlergiasUtenteObsAsync(CreateAlergiasUtenteObsRequest request);
        Task<Response<Guid>> UpdateAlergiasUtenteObsAsync(UpdateAlergiasUtenteObsRequest request, Guid id);
        Task<Response<Guid>> DeleteAlergiasUtenteObsAsync(Guid id);

    }
}
