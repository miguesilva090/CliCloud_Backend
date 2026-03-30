using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService
{
    public interface IAnamneseOrtodonticaATMService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOrtodonticaATMDTO>>> GetAnamneseOrtodonticaATMAsync(string keyword = "");
        Task<Response<AnamneseOrtodonticaATMDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOrtodonticaATMDTO>> GetAnamneseOrtodonticaATMPaginatedAsync(AnamneseOrtodonticaATMTableFilter filter);
        Task<Response<AnamneseOrtodonticaATMDTO>> GetAnamneseOrtodonticaATMAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOrtodonticaATMAsync(CreateAnamneseOrtodonticaATMRequest request);
        Task<Response<Guid>> UpdateAnamneseOrtodonticaATMAsync(UpdateAnamneseOrtodonticaATMRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOrtodonticaATMAsync(Guid id);

    }
}
