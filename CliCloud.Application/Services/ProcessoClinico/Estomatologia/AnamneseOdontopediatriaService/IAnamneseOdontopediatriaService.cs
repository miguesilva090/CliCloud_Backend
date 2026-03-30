using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService
{
    public interface IAnamneseOdontopediatriaService : ITransientService
    {
        Task<Response<IEnumerable<AnamneseOdontopediatriaDTO>>> GetAnamneseOdontopediatriaAsync(string keyword = "");
        Task<Response<AnamneseOdontopediatriaDTO?>> GetByUtenteAsync(Guid utenteId);
        Task<PaginatedResponse<AnamneseOdontopediatriaDTO>> GetAnamneseOdontopediatriaPaginatedAsync(AnamneseOdontopediatriaTableFilter filter);
        Task<Response<AnamneseOdontopediatriaDTO>> GetAnamneseOdontopediatriaAsync(Guid id);
        Task<Response<Guid>> CreateAnamneseOdontopediatriaAsync(CreateAnamneseOdontopediatriaRequest request);
        Task<Response<Guid>> UpdateAnamneseOdontopediatriaAsync(UpdateAnamneseOdontopediatriaRequest request, Guid id);
        Task<Response<Guid>> DeleteAnamneseOdontopediatriaAsync(Guid id);

    }
}
