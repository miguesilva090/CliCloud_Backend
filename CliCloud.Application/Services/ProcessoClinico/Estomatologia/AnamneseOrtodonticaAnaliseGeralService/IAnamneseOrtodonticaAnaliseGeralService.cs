using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService
{
    public interface IAnamneseOrtodonticaAnaliseGeralService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseGeralDTO>>> GetAnamneseOrtodonticaAnaliseGeralAsync(string keyword = "");
        Task<Response<AnamneseOrtodonticaAnaliseGeralDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOrtodonticaAnaliseGeralDTO>> GetAnamneseOrtodonticaAnaliseGeralPaginatedAsync(AnamneseOrtodonticaAnaliseGeralTableFilter filter);
        Task<Response<AnamneseOrtodonticaAnaliseGeralDTO>> GetAnamneseOrtodonticaAnaliseGeralAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseGeralAsync(CreateAnamneseOrtodonticaAnaliseGeralRequest request);
        Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseGeralAsync(UpdateAnamneseOrtodonticaAnaliseGeralRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseGeralAsync(Guid id);

    }
}
