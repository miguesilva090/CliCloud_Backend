using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService
{
    public interface IMapaBodyChartService : ITransientService
    {
        Task<Response<IEnumerable<MapaBodyChartDTO>>> GetMapaBodyChartAsync(string keyword = "");
        Task<Response<IEnumerable<MapaBodyChartLightDTO>>> GetMapaBodyChartLightAsync(string keyword = "");
        Task<PaginatedResponse<MapaBodyChartTableDTO>> GetMapaBodyChartPaginatedAsync(MapaBodyChartTableFilter filter);
        Task<Response<IEnumerable<MapaBodyChartTableDTO>>> GetAllMapaBodyChartAsync(MapaBodyChartAllFilter filter);
        Task<Response<MapaBodyChartDTO>> GetMapaBodyChartAsync(Guid id);
        Task<Response<MapaBodyChartDTO>> GetMapaBodyChartByNomeAsync(string nome);
        Task<Response<Guid>> CreateMapaBodyChartAsync(CreateMapaBodyChartRequest request);
        Task<Response<Guid>> UpdateMapaBodyChartAsync(UpdateMapaBodyChartRequest request, Guid id);
        Task<Response<Guid>> DeleteMapaBodyChartAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMapaBodyChartAsync(IEnumerable<Guid> ids);
    }
}
