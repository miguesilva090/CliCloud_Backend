using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.ArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.ArtigoService.Filters;

namespace CliCloud.Application.Services.Stocks.ArtigoService;

public interface IArtigoService : ITransientService
{
    Task<Response<IEnumerable<ArtigoDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<ArtigoLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<ArtigoTableDTO>> GetPaginatedAsync(ArtigoTableFilter filter);
    Task<Response<IEnumerable<ArtigoTableDTO>>> GetAllAsync(ArtigoAllFilter? filter);
    Task<Response<ArtigoDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateArtigoRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateArtigoRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}