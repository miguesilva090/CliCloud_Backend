using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService
{
    public interface IAnamneseOrtodonticaAnaliseFuncionalService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseFuncionalDTO>>> GetAnamneseOrtodonticaAnaliseFuncionalAsync(string keyword = "");
        Task<Response<AnamneseOrtodonticaAnaliseFuncionalDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOrtodonticaAnaliseFuncionalDTO>> GetAnamneseOrtodonticaAnaliseFuncionalPaginatedAsync(AnamneseOrtodonticaAnaliseFuncionalTableFilter filter);
        Task<Response<AnamneseOrtodonticaAnaliseFuncionalDTO>> GetAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseFuncionalAsync(CreateAnamneseOrtodonticaAnaliseFuncionalRequest request);
        Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseFuncionalAsync(UpdateAnamneseOrtodonticaAnaliseFuncionalRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id);

    }
}
