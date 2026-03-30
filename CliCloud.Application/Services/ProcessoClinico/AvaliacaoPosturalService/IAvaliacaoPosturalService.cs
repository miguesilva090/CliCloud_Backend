using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService
{
    public interface IAvaliacaoPosturalService : ITransientService
    {
        Task<Response<IEnumerable<AvaliacaoPosturalDTO>>> GetAvaliacaoPosturalAsync(string keyword = "");
        Task<Response<IEnumerable<AvaliacaoPosturalLightDTO>>> GetAvaliacaoPosturalLightAsync(string keyword = "");
        Task<PaginatedResponse<AvaliacaoPosturalDTO>> GetAvaliacaoPosturalPaginatedAsync(AvaliacaoPosturalTableFilter filter);
        Task<Response<IEnumerable<AvaliacaoPosturalTableDTO>>> GetAllAvaliacaoPosturalAsync(AvaliacaoPosturalAllFilter filter);
        Task<Response<AvaliacaoPosturalDTO>> GetAvaliacaoPosturalAsync(Guid id);
        Task<Response<Guid>> CreateAvaliacaoPosturalAsync(CreateAvaliacaoPosturalRequest request);
        Task<Response<Guid>> UpdateAvaliacaoPosturalAsync(UpdateAvaliacaoPosturalRequest request, Guid id);
        Task<Response<Guid>> DeleteAvaliacaoPosturalAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAvaliacaoPosturalAsync(IEnumerable<Guid> ids);
    }
}
