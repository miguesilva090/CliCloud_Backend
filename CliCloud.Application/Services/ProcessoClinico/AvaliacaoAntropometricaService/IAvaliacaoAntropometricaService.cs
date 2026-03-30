using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService
{
    public interface IAvaliacaoAntropometricaService : ITransientService
    {
        Task<Response<IEnumerable<AvaliacaoAntropometricaDTO>>> GetAvaliacaoAntropometricaAsync(string keyword = "");
        Task<Response<IEnumerable<AvaliacaoAntropometricaLightDTO>>> GetAvaliacaoAntropometricaLightAsync(string keyword = "");
        Task<PaginatedResponse<AvaliacaoAntropometricaDTO>> GetAvaliacaoAntropometricaPaginatedAsync(AvaliacaoAntropometricaTableFilter filter);
        Task<Response<IEnumerable<AvaliacaoAntropometricaTableDTO>>> GetAllAvaliacaoAntropometricaAsync(AvaliacaoAntropometricaAllFilter filter);
        Task<Response<AvaliacaoAntropometricaDTO>> GetAvaliacaoAntropometricaAsync(Guid id);
        Task<Response<Guid>> CreateAvaliacaoAntropometricaAsync(CreateAvaliacaoAntropometricaRequest request);
        Task<Response<Guid>> UpdateAvaliacaoAntropometricaAsync(UpdateAvaliacaoAntropometricaRequest request, Guid id);
        Task<Response<Guid>> DeleteAvaliacaoAntropometricaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAvaliacaoAntropometricaAsync(IEnumerable<Guid> ids);
    }
}
