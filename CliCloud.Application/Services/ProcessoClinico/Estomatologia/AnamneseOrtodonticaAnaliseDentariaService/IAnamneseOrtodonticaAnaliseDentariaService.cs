using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService
{
    public interface IAnamneseOrtodonticaAnaliseDentariaService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseDentariaDTO>>> GetAnamneseOrtodonticaAnaliseDentariaAsync(string keyword = "");
        Task<Response<AnamneseOrtodonticaAnaliseDentariaDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOrtodonticaAnaliseDentariaDTO>> GetAnamneseOrtodonticaAnaliseDentariaPaginatedAsync(AnamneseOrtodonticaAnaliseDentariaTableFilter filter);
        Task<Response<AnamneseOrtodonticaAnaliseDentariaDTO>> GetAnamneseOrtodonticaAnaliseDentariaAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseDentariaAsync(CreateAnamneseOrtodonticaAnaliseDentariaRequest request);
        Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseDentariaAsync(UpdateAnamneseOrtodonticaAnaliseDentariaRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseDentariaAsync(Guid id);

    }
}
