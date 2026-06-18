using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.ArmazemService.DTOs;
using CliCloud.Application.Services.Stocks.ArmazemService.Filters;

namespace CliCloud.Application.Services.Stocks.ArmazemService;

public interface IArmazemService : ITransientService
{
    Task<Response<IEnumerable<ArmazemDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<ArmazemLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<ArmazemTableDTO>> GetPaginatedAsync(ArmazemTableFilter filter);
    Task<Response<IEnumerable<ArmazemTableDTO>>> GetAllAsync(ArmazemAllFilter? filter);
    Task<Response<ArmazemDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateArmazemRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateArmazemRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}
