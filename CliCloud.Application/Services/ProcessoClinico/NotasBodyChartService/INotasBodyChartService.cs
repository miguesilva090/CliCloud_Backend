using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService
{
    public interface INotasBodyChartService : ITransientService
    {
        Task<Response<IEnumerable<NotasBodyChartDTO>>> GetNotasBodyChartAsync(string keyword = "");
        Task<Response<IEnumerable<NotasBodyChartLightDTO>>> GetNotasBodyChartLightAsync(string keyword = "");
        Task<PaginatedResponse<NotasBodyChartTableDTO>> GetNotasBodyChartPaginatedAsync(NotasBodyChartTableFilter filter);
        Task<Response<IEnumerable<NotasBodyChartTableDTO>>> GetAllNotasBodyChartAsync(NotasBodyChartAllFilter filter);
        Task<Response<NotasBodyChartDTO>> GetNotasBodyChartAsync(Guid id);
        Task<Response<NotasBodyChartDTO>> GetNotasBodyChartByNomeAsync(string nome);
        Task<Response<Guid>> CreateNotasBodyChartAsync(CreateNotasBodyChartRequest request);
        Task<Response<Guid>> UpdateNotasBodyChartAsync(UpdateNotasBodyChartRequest request, Guid id);
        Task<Response<Guid>> DeleteNotasBodyChartAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleNotasBodyChartAsync(IEnumerable<Guid> ids);
    }
}
