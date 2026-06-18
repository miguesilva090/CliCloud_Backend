using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Filters;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService;

public interface IFamiliaArtigoService : ITransientService
{
    Task<Response<IEnumerable<FamiliaArtigoLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<FamiliaArtigoTableDTO>> GetPaginatedAsync(FamiliaArtigoTableFilter filter);
    Task<Response<IEnumerable<FamiliaArtigoTableDTO>>> GetAllAsync(FamiliaArtigoAllFilter? filter);
    Task<Response<FamiliaArtigoDTO>> GetAsync(Guid id);
    Task<Response<IEnumerable<FamiliaArtigoBreadcrumbDTO>>> GetAncestorsAsync(Guid? parentId);
    Task<Response<Guid>> CreateAsync(CreateFamiliaArtigoRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateFamiliaArtigoRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}