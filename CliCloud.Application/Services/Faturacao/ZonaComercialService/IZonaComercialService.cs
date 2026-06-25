using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.Filters;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService;

public interface IZonaComercialService : ITransientService
{
    Task<Response<IEnumerable<ZonaComercialDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<ZonaComercialLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<ZonaComercialTableDTO>> GetPaginatedAsync(ZonaComercialTableFilter filter);
    Task<Response<IEnumerable<ZonaComercialTableDTO>>> GetAllAsync(ZonaComercialAllFilter? filter);
    Task<Response<ZonaComercialDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateZonaComercialRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateZonaComercialRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}