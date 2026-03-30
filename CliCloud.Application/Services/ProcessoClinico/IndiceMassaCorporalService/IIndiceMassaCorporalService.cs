using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.IndiceMassaCorporalService.DTOs;
using CliCloud.Application.Services.IndiceMassaCorporalService.Filters;

namespace CliCloud.Application.Services.IndiceMassaCorporalService
{
    public interface IIndiceMassaCorporalService : ITransientService
    {
        Task<Response<IEnumerable<IndiceMassaCorporalDTO>>> GetIndiceMassaCorporalAsync(string keyword = "");
        Task<PaginatedResponse<IndiceMassaCorporalDTO>> GetIndiceMassaCorporalPaginatedAsync(IndiceMassaCorporalTableFilter filter);
        Task<Response<IndiceMassaCorporalDTO>> GetIndiceMassaCorporalAsync(Guid id);
        Task<Response<Guid>> CreateIndiceMassaCorporalAsync(CreateIndiceMassaCorporalRequest request);
        Task<Response<Guid>> UpdateIndiceMassaCorporalAsync(UpdateIndiceMassaCorporalRequest request, Guid id);
        Task<Response<Guid>> DeleteIndiceMassaCorporalAsync(Guid id);

    }
}
