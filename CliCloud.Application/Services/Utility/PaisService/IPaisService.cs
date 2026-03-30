using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.Filters;

namespace CliCloud.Application.Services.Utility.PaisService
{
    public interface IPaisService : ITransientService
    {
        Task<Response<IEnumerable<PaisDTO>>> GetPaisAsync(string keyword = "");
        Task<Response<IEnumerable<PaisLightDTO>>> GetPaisLightAsync(string keyword = "");
        Task<PaginatedResponse<PaisTableDTO>> GetPaisPaginatedAsync(PaisTableFilter filter);
        Task<Response<IEnumerable<PaisTableDTO>>> GetAllPaisAsync(PaisAllFilter filter);
        Task<Response<PaisDTO>> GetPaisAsync(Guid id);
        Task<Response<Guid>> CreatePaisAsync(CreatePaisRequest request);
        Task<Response<Guid>> UpdatePaisAsync(UpdatePaisRequest request, Guid id);
        Task<Response<Guid>> DeletePaisAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultiplePaisAsync(IEnumerable<Guid> ids);
    }
}
