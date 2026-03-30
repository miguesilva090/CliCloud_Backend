using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TemperaturaCorporalService.DTOs;
using CliCloud.Application.Services.TemperaturaCorporalService.Filters;

namespace CliCloud.Application.Services.TemperaturaCorporalService
{
    public interface ITemperaturaCorporalService : ITransientService
    {
        Task<Response<IEnumerable<TemperaturaCorporalDTO>>> GetTemperaturaCorporalAsync(string keyword = "");
        Task<PaginatedResponse<TemperaturaCorporalDTO>> GetTemperaturaCorporalPaginatedAsync(TemperaturaCorporalTableFilter filter);
        Task<Response<TemperaturaCorporalDTO>> GetTemperaturaCorporalAsync(Guid id);
        Task<Response<Guid>> CreateTemperaturaCorporalAsync(CreateTemperaturaCorporalRequest request);
        Task<Response<Guid>> UpdateTemperaturaCorporalAsync(UpdateTemperaturaCorporalRequest request, Guid id);
        Task<Response<Guid>> DeleteTemperaturaCorporalAsync(Guid id);

    }
}
