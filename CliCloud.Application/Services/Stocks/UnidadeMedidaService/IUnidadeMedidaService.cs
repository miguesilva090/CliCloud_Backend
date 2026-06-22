using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Filters;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService;

public interface IUnidadeMedidaService : ITransientService
{
    Task<Response<IEnumerable<UnidadeMedidaDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<UnidadeMedidaLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<UnidadeMedidaTableDTO>> GetPaginatedAsync(UnidadeMedidaTableFilter filter);
    Task<Response<IEnumerable<UnidadeMedidaTableDTO>>> GetAllAsync(UnidadeMedidaAllFilter? filter);
    Task<Response<UnidadeMedidaDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateUnidadeMedidaRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateUnidadeMedidaRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}
