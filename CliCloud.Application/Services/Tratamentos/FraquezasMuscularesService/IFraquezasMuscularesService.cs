using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Filters;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService
{
    public interface IFraquezasMuscularesService : ITransientService
    {
        Task<Response<IEnumerable<FraquezasMuscularesDTO>>> GetFraquezasMuscularesAsync(string keyword = "");
        Task<Response<IEnumerable<FraquezasMuscularesLightDTO>>> GetFraquezasMuscularesLightAsync(string keyword = "");
        Task<Response<IEnumerable<FraquezasMuscularesTableDTO>>> GetAllFraquezasMuscularesAsync(FraquezasMuscularesAllFilter filter);
        Task<PaginatedResponse<FraquezasMuscularesTableDTO>> GetFraquezasMuscularesPaginatedAsync(FraquezasMuscularesTableFilter filter);
        Task<Response<FraquezasMuscularesDTO>> GetFraquezasMuscularesAsync(Guid id);
        Task<Response<FraquezasMuscularesDTO>> GetFraquezasMuscularesByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateFraquezasMuscularesAsync(CreateFraquezasMuscularesRequest request);
        Task<Response<Guid>> UpdateFraquezasMuscularesAsync(UpdateFraquezasMuscularesRequest request, Guid id);
        Task<Response<Guid>> DeleteFraquezasMuscularesAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleFraquezasMuscularesAsync(IEnumerable<Guid> ids);
    }
}
