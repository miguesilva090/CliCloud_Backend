using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.Filters;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService
{
    public interface ICentroSaudeService : ITransientService
    {
        Task<Response<IEnumerable<CentroSaudeDTO>>> GetCentroSaudeAsync(string keyword = "");
        Task<Response<IEnumerable<CentroSaudeLightDTO>>> GetCentroSaudeLightAsync(string keyword = "");
        Task<PaginatedResponse<CentroSaudeTableDTO>> GetCentroSaudePaginatedAsync(CentroSaudeTableFilter filter);
        Task<Response<IEnumerable<CentroSaudeTableDTO>>> GetAllCentroSaudeAsync(CentroSaudeAllFilter filter);
        Task<Response<CentroSaudeDTO>> GetCentroSaudeAsync(Guid id);
        Task<Response<CentroSaudeDTO>> GetCentroSaudeByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<CentroSaudeDTO>>> GetCentroSaudeByNameAsync(string nome);
        Task<Response<Guid>> CreateCentroSaudeAsync(CreateCentroSaudeRequest request);
        Task<Response<Guid>> UpdateCentroSaudeAsync(UpdateCentroSaudeRequest request, Guid id);
        Task<Response<Guid>> DeleteCentroSaudeAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleCentroSaudeAsync(IEnumerable<Guid> ids);
    }
}
