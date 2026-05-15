using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Filters;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService
{
    public interface ILoteDirectService : ITransientService
    {
        Task<PaginatedResponse<LoteDirectTableDTO>> GetPaginatedAsync(LoteDirectTableFilter filter);
        Task<Response<LoteDirectDTO>> GetByIdAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateLoteDirectRequest request);
        Task<Response<Guid>> UpdateAsync(Guid id, UpdateLoteDirectRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<int>> CorrigirLotesAsync(CorrigirLotesRequest request);
        Task<Response<IEnumerable<TipoLoteLightDTO>>> GetTiposLoteLightAsync();
    }
}