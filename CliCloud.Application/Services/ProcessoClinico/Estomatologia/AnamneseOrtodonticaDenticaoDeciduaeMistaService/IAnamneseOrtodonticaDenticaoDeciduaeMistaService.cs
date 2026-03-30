using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService
{
    public interface IAnamneseOrtodonticaDenticaoDeciduaeMistaService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(string keyword = "");
        Task<Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaPaginatedAsync(AnamneseOrtodonticaDenticaoDeciduaeMistaTableFilter filter);
        Task<Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(CreateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request);
        Task<Response<Guid>> UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id);

    }
}
